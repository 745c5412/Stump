using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Fights.History;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes;
using Stump.Server.WorldServer.Game.Spells;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Targets;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public sealed class CharacterFighter : NamedFighter
    {
        private int m_criticalWeaponBonus;
        private int m_damageTakenBeforeFight;
        private short m_earnedDishonor;
        private int m_earnedExp;
        private int m_guildEarnedExp;
        private short m_earnedHonor;
        private int m_weaponUses;
        private bool m_isUsingWeapon;




        public CharacterFighter(Character character, FightTeam team)
            : base(team)
        {
            Character = character;

            Look = Character.Look.Clone();
            Look.RemoveAuras();

            Cell cell;
            if (!Fight.FindRandomFreeCell(this, out cell, false))
                return;

            Position = new ObjectPosition(character.Map, cell, character.Direction);

            InitializeCharacterFighter();
        }

        public Character Character
        {
            get;
            private set;
        }

        public ReadyChecker PersonalReadyChecker
        {
            get;
            set;
        }

        public override int Id
        {
            get { return Character.Id; }
        }

        public override string Name
        {
            get { return Character.Name; }
        }

        public override ActorLook Look
        {
            get;
            set;
        }

        public override ObjectPosition MapPosition
        {
            get { return Character.Position; }
        }

        public override byte Level
        {
            get { return Character.Level; }
        }

        public override StatsFields Stats
        {
            get { return Character.Stats; }
        }

        public bool IsDisconnected
        {
            get;
            private set;
        }

        public int LeftRound
        {
            get;
            private set;
        }

        private void InitializeCharacterFighter()
        {
            m_damageTakenBeforeFight = Stats.Health.DamageTaken;
        }

        public override ObjectPosition GetLeaderBladePosition()
        {
            return Character.GetPositionBeforeMove();
        }

        public void ToggleTurnReady(bool ready)
        {
            if (PersonalReadyChecker != null)
                PersonalReadyChecker.ToggleReady(this, ready);

            else if (Fight.ReadyChecker != null)
                Fight.ReadyChecker.ToggleReady(this, ready);
        }

        #region Leave

        public void LeaveFight(bool force = false)
        {
            if (HasLeft())
                return;

            m_left = !force;

            OnLeft();
        }


        private bool m_left;
        public override bool HasLeft()
        {
            return m_left;
        }

        public override bool CanPlay()
        {
            return base.CanPlay() && (!HasLeft() || IsDisconnected);
        }

        #endregion

        public override void ResetUsedPoints()
        {
            base.ResetUsedPoints();
            m_weaponUses = 0;
        }

        public override bool CastSpell(Spell spell, Cell cell, bool force = false, bool apFree = false)
        {
            if (!IsFighterTurn() && !force)
                return false;

            // weapon attack
            if (spell.Id != 0 ||
                Character.Inventory.TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON) == null)
                return base.CastSpell(spell, cell, force, apFree);

            var weapon = Character.Inventory.TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON);
            var weaponTemplate =  weapon.Template as WeaponTemplate;

            if (weaponTemplate == null || !CanUseWeapon(cell, weaponTemplate))
            {
                OnSpellCastFailed(spell, cell);
                return false;
            }  

            Fight.StartSequence(SequenceTypeEnum.SEQUENCE_WEAPON);

            var random = new AsyncRandom();
            var critical = RollCriticalDice(weaponTemplate);

            switch (critical)
            {
                case FightSpellCastCriticalEnum.CRITICAL_FAIL:
                    OnWeaponUsed(weaponTemplate, cell, critical, false);

                    if (!apFree)
                        UseAP((short) weaponTemplate.ApCost);
                
                    Fight.EndSequence(SequenceTypeEnum.SEQUENCE_WEAPON);

                    PassTurn();

                    return false;
                case FightSpellCastCriticalEnum.CRITICAL_HIT:
                    m_criticalWeaponBonus = weaponTemplate.CriticalHitBonus;
                    break;
            }

            m_isUsingWeapon = true;
            var effects =
                weapon.Effects.Where(entry => EffectManager.Instance.IsUnRandomableWeaponEffect(entry.EffectId)).OfType<EffectDice>();
            var handlers = new List<SpellEffectHandler>();
            foreach (var effect in effects)
            {
                if (effect.Random > 0)
                {
                    if (random.NextDouble() > effect.Random/100d)
                    {
                        // effect ignored
                        continue;
                    }
                }

                var handler = EffectManager.Instance.GetSpellEffectHandler(effect, this, spell, cell,
                    critical == FightSpellCastCriticalEnum.CRITICAL_HIT);
                handler.EffectZone = new Zone(weaponTemplate.Type.ZoneShape, (byte) weaponTemplate.Type.ZoneSize,
                    handler.CastPoint.OrientationTo(handler.TargetedPoint), (int)weaponTemplate.Type.ZoneEfficiencyPercent, (int)weaponTemplate.Type.ZoneMaxEfficiency);
                handler.Targets = new TargetCriterion[]
                    { new TargetTypeCriterion(SpellTargetType.ALLY_ALL), new TargetTypeCriterion(SpellTargetType.ENEMY_ALL) }; // everyone but caster
                handlers.Add(handler);
            }

            var silentCast = handlers.Any(entry => entry.RequireSilentCast());

            OnWeaponUsed(weaponTemplate, cell, critical, silentCast);

            if (!apFree)
                UseAP((short) weaponTemplate.ApCost);

            foreach (var handler in handlers)
                handler.Apply();

            Fight.EndSequence(SequenceTypeEnum.SEQUENCE_WEAPON);

            m_isUsingWeapon = false;
            m_criticalWeaponBonus = 0;

            // is it the right place to do that ?
            Fight.CheckFightEnd();

            return true;
        }

        protected override void OnWeaponUsed(WeaponTemplate weapon, Cell cell, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            m_weaponUses++;
            base.OnWeaponUsed(weapon, cell, critical, silentCast);
        }

        public override SpellCastResult CanCastSpell(Spell spell, Cell cell)
        {
             var result = base.CanCastSpell(spell, cell);

            if (result == SpellCastResult.OK)
                return result;

            switch (result)
            {
                case SpellCastResult.NO_LOS:
                    // Impossible de lancer ce sort : un obstacle g�ne votre vue !
                    Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 174);
                    break;
                case SpellCastResult.HAS_NOT_SPELL:
                    // Impossible de lancer ce sort : vous ne le poss�dez pas !
                    Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 169);
                    break;
                case SpellCastResult.NOT_ENOUGH_AP:
                    // Impossible de lancer ce sort : Vous avez %1 PA disponible(s) et il vous en faut %2 pour ce sort !
                    Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 170, AP, spell.CurrentSpellLevel.ApCost);
                    break;
                case SpellCastResult.UNWALKABLE_CELL:
                    // Impossible de lancer ce sort : la cellule vis�e n'est pas disponible !
                    Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 172);
                    break;
                case SpellCastResult.CELL_NOT_FREE:
                    //Impossible de lancer ce sort : la cellule vis�e n'est pas valide !
                    Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 193);
                    break;
                default:
                    Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 175);
                    Character.SendServerMessage("(" + result + ")", Color.Red);
                    break;
            }

            return result;
        }


        public override int CalculateDamage(int damage, EffectSchoolEnum type, bool critical)
        {
            if (Character.GodMode)
                return short.MaxValue;

            return
                base.CalculateDamage(
                ((m_isUsingWeapon ? m_criticalWeaponBonus + Stats[PlayerFields.WeaponDamageBonus] : 0) + damage),
                    type, critical);
        }

        public bool CanUseWeapon(Cell cell, WeaponTemplate weapon)
        {
            if (!IsFighterTurn())
                return false;

            if (HasState((int)SpellStatesEnum.AFFAIBLI))
                return false;

            var point = new MapPoint(cell);

            if ((weapon.CastInDiagonal && (point.EuclideanDistanceTo(Position.Point) > weapon.WeaponRange || point.EuclideanDistanceTo(Position.Point) < weapon.MinRange)) ||
                (!weapon.CastInDiagonal && point.ManhattanDistanceTo(Position.Point) > weapon.WeaponRange || point.ManhattanDistanceTo(Position.Point) < weapon.MinRange))
                return false;

            if (m_weaponUses >= weapon.MaxCastPerTurn)
                return false;

            return AP >= weapon.ApCost && Fight.CanBeSeen(cell, Position.Cell);
        }

        public override Spell GetSpell(int id)
        {
            return Character.Spells.GetSpell(id);
        }

        public override bool HasSpell(int id)
        {
            return Character.Spells.HasSpell(id);
        }

        public bool IsSlaveTurn()
        {
            var slave = Fight.TimeLine.Current as SlaveFighter;

            if (slave == null)
                return false;

            return slave.Summoner == this;
        }

        public SlaveFighter GetSlave()
        {
            var slave = Fight.TimeLine.Current as SlaveFighter;

            if (slave == null)
                return null;

            return slave.Summoner == this ? slave : null;
        }

        public FightSpellCastCriticalEnum RollCriticalDice(WeaponTemplate weapon)
        {
            var random = new AsyncRandom();

            var critical = FightSpellCastCriticalEnum.NORMAL;

            if (weapon.CriticalHitProbability != 0 && random.NextDouble()*100 < weapon.CriticalFailureProbability + Stats[PlayerFields.CriticalMiss])
                critical = FightSpellCastCriticalEnum.CRITICAL_FAIL;

            else if (weapon.CriticalHitProbability != 0 &&
                     random.NextDouble() * 100 < weapon.CriticalHitProbability + Stats[PlayerFields.CriticalHit])
                critical = FightSpellCastCriticalEnum.CRITICAL_HIT;

            return critical;
        }

        public override void ResetFightProperties()
        {
            base.ResetFightProperties();

            if (Fight.IsDeathTemporarily)
                Stats.Health.DamageTaken = m_damageTakenBeforeFight;
            else if (Stats.Health.Total <= 0)
                Stats.Health.DamageTaken = (Stats.Health.TotalMax - 1);
        }

        public override bool MustSkipTurn()
        {
            return base.MustSkipTurn() || (IsDisconnected && Team.GetAllFighters<CharacterFighter>().Any(x => x.CanPlay() && !x.IsDisconnected));
        }

        public void EnterDisconnectedState()
        {
            IsDisconnected = true;
            LeftRound = Fight.TimeLine.RoundNumber;
        }

        public void LeaveDisconnectedState(bool left = true)
        {
            IsDisconnected = false;

            if (left)
                m_left = false;
        }

        public void RestoreFighterFromDisconnection(Character character)
        {
            if (!IsDisconnected)
            {
                throw new Exception("Fighter wasn't disconnected");
            }

            Character.Stats.CopyContext(character.Stats);
            Character = character;
        }

        public override IFightResult GetFightResult(FightOutcomeEnum outcome)
        {
            return new FightPlayerResult(this, outcome, Loot);
        }

        public override FightTeamMemberInformations GetFightTeamMemberInformations()
        {
            return new FightTeamMemberCharacterInformations(Id, Name, Character.Level);
        }

        public override GameFightFighterInformations GetGameFightFighterInformations(WorldClient client = null)
        {
            return new GameFightCharacterInformations(Id,
                                                      Look.GetEntityLook(),
                                                      GetEntityDispositionInformations(client),
                                                      (sbyte)Team.Id,
                                                      0,
                                                      IsAlive(),
                                                      GetGameFightMinimalStats(client),
                                                      new short[0],
                                                      Name,
                                                      Character.GetPlayerStatus(),
                                                      Character.Level,
                                                      Character.GetActorAlignmentInformations(),
                                                      (sbyte) Character.Breed.Id,
                                                      Character.Sex != SexTypeEnum.SEX_MALE);
        }

        public override GameFightFighterLightInformations GetGameFightFighterLightInformations(WorldClient client = null)
            => new GameFightFighterNamedLightInformations(
                Character.Sex == SexTypeEnum.SEX_FEMALE,
                IsAlive(),
                Id,
                0,
                Level,
                (sbyte)Character.Breed.Id,
                Name);

        public override string ToString()
        {
            return Character.ToString();
        }

        #region God state
        public override bool UseAP(short amount)
        {
            if (!Character.GodMode)
                return base.UseAP(amount);

            base.UseAP(amount);
            RegainAP(amount);

            return true;
        }

        public override bool UseMP(short amount)
        {
            return Character.GodMode || base.UseMP(amount);
        }

        public override bool LostAP(short amount, FightActor source)
        {
            if (!Character.GodMode)
                return base.LostAP(amount, source);

            base.LostAP(amount, source);
            RegainAP(amount);

            return true;
        }

        public override bool LostMP(short amount, FightActor source)
        {
            return Character.GodMode || base.LostMP(amount, source);
        }


        public override int InflictDamage(Damage damage)
        {
            if (!Character.GodMode)
                return base.InflictDamage(damage);

            damage.GenerateDamages();
            OnBeforeDamageInflicted(damage);

            damage.Source.TriggerBuffs(BuffTriggerType.BEFORE_ATTACK, damage);
            TriggerBuffs(BuffTriggerType.BEFORE_ATTACKED, damage);

            OnDamageReducted(damage.Source, damage.Amount);

            damage.Source.TriggerBuffs(BuffTriggerType.AFTER_ATTACK, damage);
            TriggerBuffs(BuffTriggerType.AFTER_ATTACKED, damage);

            OnDamageInflicted(damage);

            return 0;
        }

        #endregion
    }
}