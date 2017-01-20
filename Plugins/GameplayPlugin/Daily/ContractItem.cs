using System;
using System.Collections.ObjectModel;
using System.Linq;
using Stump.Core.Attributes;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.Player;

namespace GameplayPlugin.Daily
{
    public class ContractItemRegistrer : Singleton<ContractItemRegistrer>
    {
        [Initialization(typeof (ItemManager))]
        public void Initialize()
        {
            ItemManager.Instance.AddItemConstructorById((ItemIdEnum) DailyQuestManager.ContractItemId, typeof (ContractItem));
        }
    }

    public class ContractItem : BasePlayerItem
    {
        public static readonly EffectsEnum EffectDescription = (EffectsEnum) 2002;

        public static readonly EffectsEnum EffectContractItem = (EffectsEnum) 2000;

        public static readonly EffectsEnum EffectContractValidated = (EffectsEnum) 2001;

        private int[] m_completions;

        private EffectString[] m_descriptionEffects;

        private bool m_initialized;
        private DailyObjectiveRecord[] m_objectives;
        private EffectDice[] m_objectivesEffects;
        private EffectDate m_validatedOn;

        public ContractItem(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
            InitializeEffects();
        }

        public override bool CanBeDestroyed()
        {
            return false;
        }

        public DailyObjectiveRecord[] Objectives => m_objectives ?? (m_objectives = m_objectivesEffects.Select(x => DailyQuestManager.Instance.GetObjective(x.DiceNum)).ToArray());

        public ReadOnlyCollection<int> Completions => new ReadOnlyCollection<int>(m_completions);

        public DateTime? ContractValidationDate
        {
            get { return m_validatedOn?.GetDate(); }
            set
            {
                if (value == null)
                    Effects.Remove(m_validatedOn);

                if (m_validatedOn == null && value != null)
                    Effects.Add(m_validatedOn = new EffectDate(EffectContractValidated, value.Value));
                else
                    m_validatedOn = value == null ? null : new EffectDate(EffectContractValidated, value.Value);


                Invalidate();
                Owner.Inventory.RefreshItem(this);
            }
        }

        public bool HasBeenValidatedToday() => ContractValidationDate != null && (DateTime.Now.DayOfYear == ContractValidationDate.Value.DayOfYear && DateTime.Now.Year == ContractValidationDate.Value.Year);

        public bool IsFinished()
        {
            if (ContractValidationDate != null)
                return true;

            var i = 0;

            while (i < Objectives.Length && Completions[i] >= Objectives[i].Amount)
                i++;

            return i == Objectives.Length;
        }

        private void IncreaseCounter(int objectiveIndex, int amount)
        {
            if (objectiveIndex > m_completions.Length)
                return;

            if (m_completions[objectiveIndex] + amount > Objectives[objectiveIndex].Amount)
            {
                m_completions[objectiveIndex] = Objectives[objectiveIndex].Amount;
                m_objectivesEffects[objectiveIndex].DiceFace = (short) Objectives[objectiveIndex].Amount;
            }
            else
            {
                m_completions[objectiveIndex] += amount;
                m_objectivesEffects[objectiveIndex].DiceFace += (short) amount;
            }

            m_descriptionEffects[objectiveIndex].Text = $"{Objectives[objectiveIndex].Name} : {m_completions[objectiveIndex]} / {Objectives[objectiveIndex].Amount}";

            Invalidate();
        }

        private void UpdateItemCounter(int objectiveIndex, ItemTemplate item)
        {
            var count = Owner.Inventory.Where(x => x.Template == item && !x.IsEquiped()).Sum(x => x.Stack);

            m_completions[objectiveIndex] = (int) (count > Objectives[objectiveIndex].Amount ? Objectives[objectiveIndex].Amount : count);
            m_objectivesEffects[objectiveIndex].DiceFace = (short) (count > Objectives[objectiveIndex].Amount ? Objectives[objectiveIndex].Amount : count);

            m_descriptionEffects[objectiveIndex].Text = $"{Objectives[objectiveIndex].Name} : {m_completions[objectiveIndex]} / {Objectives[objectiveIndex].Amount}";

            Invalidate();
        }

        private void InitializeEffects()
        {
            if (Effects.Count <= 1 || m_initialized)
                return;

            m_objectivesEffects = Effects.OfType<EffectDice>().Where(x => x.EffectId == EffectContractItem).ToArray();
            m_completions = m_objectivesEffects.Select(x => (int) x.DiceFace).ToArray();
            m_validatedOn = Effects.OfType<EffectDate>().FirstOrDefault(x => x.EffectId == EffectContractValidated);
            m_descriptionEffects = Effects.OfType<EffectString>().ToArray();

            if (m_validatedOn != null)
            {
                m_initialized = true;
                return;
            }

            m_initialized = true;

            if (!IsFinished())
                SubscribeEvents();
        }

        private void OnInventoryChanged(BasePlayerItem item)
        {
            var refresh = false;
            for (var i = 0; i < Objectives.Length; i++)
            {
                if (item.Template == Objectives[i].Item)
                {
                    UpdateItemCounter(i, item.Template);
                    refresh = true;
                }
            }

            if (IsFinished())
            {
                UnSubscribeEvents();
            }

            if (refresh)
                Owner.Inventory.RefreshItem(this);
        }

        private void OnItemAdded(ItemsCollection<BasePlayerItem> sender, BasePlayerItem item)
        {
            OnInventoryChanged(item);
        }

        private void OnItemStackChanged(ItemsCollection<BasePlayerItem> sender, BasePlayerItem item, int difference)
        {
            OnInventoryChanged(item);
        }

        private void OnItemRemoved(ItemsCollection<BasePlayerItem> sender, BasePlayerItem item)
        {
            OnInventoryChanged(item);
        }

        private void OnFightEnded(Character character, CharacterFighter fighter)
        {
            var refresh = false;
            for (var i = 0; i < Objectives.Length; i++)
            {
                var monster = Objectives[i].Monster;

                if (monster == null)
                    continue;

                var count = fighter.OpposedTeam.Fighters.OfType<MonsterFighter>().Count(x => x.Monster.Template == monster);

                if (count > 0)
                {
                    IncreaseCounter(i, count);
                    refresh = true;
                }
            }

            if (IsFinished())
            {
                UnSubscribeEvents();
            }

            if (refresh)
                Owner.Inventory.RefreshItem(this);
        }

        public void InitializeEffects(DailyObjectiveRecord[] objectives)
        {
            if (m_initialized)
                return;

            Effects.Clear();
            Effects.AddRange(
                m_objectivesEffects = objectives.Select((x, i) => new EffectDice((short) EffectContractItem, (short) objectives[i].Amount, (short) x.Id, 0, new EffectBase {Hidden = true})).ToArray());
            Effects.AddRange(
                m_descriptionEffects = objectives.Select((x, i) => new EffectString((short) EffectDescription, $"{x.Name} : 0 / {x.Amount}", new EffectBase())).ToArray());

            m_completions = new int[m_objectivesEffects.Length];

            m_initialized = true;

            if (!IsFinished())
                SubscribeEvents();

            Invalidate();
            Owner.Inventory.RefreshItem(this);
        }

        private void SubscribeEvents()
        {
            Owner.FightEnded += OnFightEnded;
            Owner.Inventory.ItemAdded += OnItemAdded;
            Owner.Inventory.ItemStackChanged += OnItemStackChanged;
            Owner.Inventory.ItemRemoved += OnItemRemoved;
        }

        private void UnSubscribeEvents()
        {
            Owner.FightEnded -= OnFightEnded;
            Owner.Inventory.ItemAdded -= OnItemAdded;
            Owner.Inventory.ItemStackChanged -= OnItemStackChanged;
            Owner.Inventory.ItemRemoved -= OnItemRemoved;
        }

        public override bool OnRemoveItem()
        {
            Owner.FightEnded -= OnFightEnded;
            return base.OnRemoveItem();
        }
    }
}