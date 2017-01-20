using MongoDB.Bson;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Logging;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Handlers.Context;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class FightAgression : Fight<FightPlayerTeam, FightPlayerTeam>
    {
        public FightAgression(int id, Map fightMap, FightPlayerTeam defendersTeam, FightPlayerTeam challengersTeam)
            : base(id, fightMap, defendersTeam, challengersTeam)
        {
            m_placementTimer = Map.Area.CallDelayed(FightConfiguration.PlacementPhaseTime, StartFighting);
        }

        public override void StartPlacement()
        {
            base.StartPlacement();

            m_placementTimer = Map.Area.CallDelayed(FightConfiguration.PlacementPhaseTime, StartFighting);
        }

        public override void StartFighting()
        {
            m_placementTimer.Dispose();

            base.StartFighting();
        }

        public override FightTypeEnum FightType => FightTypeEnum.FIGHT_TYPE_AGRESSION;

        public override bool IsPvP => true;

        public override bool IsMultiAccountRestricted => true;

        protected override void ApplyResults()
        {
            foreach (var fightResult in Results)
            {
                fightResult.Apply();
            }
        }

        protected override List<IFightResult> GetResults()
        {
            var results = GetFightersAndLeavers().Where(entry => entry.HasResult).
                 Select(fighter => fighter.GetFightResult()).ToList();

            foreach (var playerResult in results.OfType<FightPlayerResult>())
            {
                playerResult.SetEarnedHonor(CalculateEarnedHonor(playerResult.Fighter),
                    CalculateEarnedDishonor(playerResult.Fighter));

                CalculateEarnedPevetons(playerResult);

                var document = new BsonDocument
                    {
                        { "FightId", UniqueId.ToString() },
                        { "FightType", Enum.GetName(typeof(FightTypeEnum), FightType) },
                        { "Duration", GetFightDuration() },
                        { "Team", Enum.GetName(typeof(TeamEnum), playerResult.Fighter.Team.Id) },
                        { "Win", Winners.Id == playerResult.Fighter.Team.Id },
                        { "AcctId", playerResult.Character.Account.Id },
                        { "AcctName", playerResult.Character.Account.Login },
                        { "CharacterId", playerResult.Character.Id },
                        { "CharacterName", playerResult.Character.Name },
                        { "IPAddress", playerResult.Character.Client.IP },
                        { "ClientKey", playerResult.Character.Account.LastClientKey },
                        { "Date", DateTime.Now.ToString(CultureInfo.InvariantCulture) }
                    };

                MongoLogger.Instance.Insert("fights_results", document);
            }

            return results;
        }

        protected override void SendGameFightJoinMessage(CharacterFighter fighter)
        {
            ContextHandler.SendGameFightJoinMessage(fighter.Character.Client, CanCancelFight(), !IsStarted, false, IsStarted, GetPlacementTimeLeft(), FightType);
        }

        protected override void SendGameFightJoinMessage(FightSpectator spectator)
        {
            ContextHandler.SendGameFightJoinMessage(spectator.Character.Client, false, false, true, IsStarted, GetPlacementTimeLeft(), FightType);
        }

        public override int GetPlacementTimeLeft()
        {
            var timeleft = FightConfiguration.PlacementPhaseTime - (DateTime.Now - CreationTime).TotalMilliseconds;

            if (timeleft < 0)
                timeleft = 0;

            return (int)timeleft;
        }

        protected override bool CanCancelFight()
        {
            return false;
        }

        private void CalculateEarnedPevetons(FightPlayerResult result)
        {
            var pvpSeek = result.Character.Inventory.GetItems(x => x.Template.Id == (int)ItemIdEnum.OrderofExecution).FirstOrDefault();

            if (pvpSeek != null && ChallengersTeam.Fighters.Contains(result.Fighter))
            {
                var seekEffect = pvpSeek.Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_Seek) as EffectString;

                if (seekEffect != null)
                {
                    var target = result.Fighter.OpposedTeam.GetAllFightersWithLeavers<CharacterFighter>().FirstOrDefault(x => x.Name == seekEffect.Text);

                    if (target != null)
                    {
                        result.Character.Inventory.RemoveItem(pvpSeek);

                        if (Winners == result.Fighter.Team)
                        {
                            result.Loot.AddItem((int)ItemIdEnum.Stroken, 2);
                            result.Character.SendServerMessage("Vous avez abattu votre cible avec succès, vous avez gagné 2 Pévétons !");
                        }
                        else
                        {
                            target.Loot.AddItem((int)ItemIdEnum.Stroken, 2);
                            target.Character.SendServerMessage("Vous avez vaincu votre traqueur, vous avez gagné 2 Pévétons !");
                        }
                    }
                }
                else
                    result.Character.Inventory.RemoveItem(pvpSeek);
            }
        }

        public short CalculateEarnedHonor(CharacterFighter character)
        {
            if (Draw)
                return 0;

            if (character.OpposedTeam.AlignmentSide == AlignmentSideEnum.ALIGNMENT_NEUTRAL)
                return 0;

            var winnersLevel = (double)Winners.GetAllFightersWithLeavers<CharacterFighter>().Sum(entry => entry.Level);
            var losersLevel = (double)Losers.GetAllFightersWithLeavers<CharacterFighter>().Sum(entry => entry.Level);
            var maxLosersLevel = winnersLevel + 15;

            var delta = Math.Floor(Math.Sqrt(character.Level) * 10 * ((losersLevel > maxLosersLevel ? maxLosersLevel : losersLevel) / winnersLevel));

            if (Losers == character.Team)
                delta = -(character.Character.Honor * 0.10);

            return (short)delta;
        }

        public short CalculateEarnedDishonor(CharacterFighter character)
        {
            if (Draw)
                return 0;

            return character.OpposedTeam.AlignmentSide != AlignmentSideEnum.ALIGNMENT_NEUTRAL ? (short)0 : (short)1;
        }
    }
}