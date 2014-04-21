﻿using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.BaseServer.IPC.Messages;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Items.Player
{
    /// <summary>
    ///   Represents the Inventory of a character
    /// </summary>
    public sealed class Inventory : ItemsStorage<BasePlayerItem>, IDisposable
    {
        [Variable(true)]
        private const int MaxInventoryKamas = 150000000;

        [Variable]
        public static readonly bool ActiveTokens = true;

        [Variable]
        public static readonly int TokenTemplateId = (int)ItemIdEnum.GameMasterToken;
        private static ItemTemplate TokenTemplate;

        [Initialization(typeof(ItemManager), Silent=true)]
        private static void InitializeTokenTemplate()
        {
            if (ActiveTokens)
                TokenTemplate = ItemManager.Instance.TryGetTemplate(TokenTemplateId);
        }

        #region Events

        #region Delegates

        public delegate void ItemMovedEventHandler(Inventory sender, BasePlayerItem item, CharacterInventoryPositionEnum lastPosition);

        #endregion

        public event ItemMovedEventHandler ItemMoved;

        public void NotifyItemMoved(BasePlayerItem item, CharacterInventoryPositionEnum lastPosition)
        {
            OnItemMoved(item, lastPosition);

            var handler = ItemMoved;
            if (handler != null) handler(this, item, lastPosition);
        }


        #endregion

        private readonly Dictionary<CharacterInventoryPositionEnum, List<BasePlayerItem>> m_itemsByPosition
            = new Dictionary<CharacterInventoryPositionEnum, List<BasePlayerItem>>
                  {
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_CAPE, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_BELT, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_BOOTS, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_AMULET, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_LEFT, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_RIGHT, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_1, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_2, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_3, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_4, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_5, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_6, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_MOUNT, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_MUTATION, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_BOOST_FOOD, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_FIRST_BONUS, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_SECOND_BONUS, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_FIRST_MALUS, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_SECOND_MALUS, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_ROLEPLAY_BUFFER, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_FOLLOWER, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED, new List<BasePlayerItem>()},
                  };

        private readonly Dictionary<ItemSuperTypeEnum, CharacterInventoryPositionEnum[]> m_itemsPositioningRules
            = new Dictionary<ItemSuperTypeEnum, CharacterInventoryPositionEnum[]>
            {
                {ItemSuperTypeEnum.SUPERTYPE_AMULET, new[] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_AMULET}},
                {ItemSuperTypeEnum.SUPERTYPE_WEAPON, new[] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON}},
                {ItemSuperTypeEnum.SUPERTYPE_WEAPON_8, new[] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON}},
                {ItemSuperTypeEnum.SUPERTYPE_CAPE, new[] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_CAPE}},
                {ItemSuperTypeEnum.SUPERTYPE_HAT, new[] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT}},
                {
                    ItemSuperTypeEnum.SUPERTYPE_RING,
                    new[]
                    {
                        CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_LEFT,
                        CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_RIGHT
                    }
                },
                {ItemSuperTypeEnum.SUPERTYPE_BOOTS, new[] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_BOOTS}},
                {ItemSuperTypeEnum.SUPERTYPE_BELT, new[] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_BELT}},
                {ItemSuperTypeEnum.SUPERTYPE_PET, new[] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS}},
                {
                    ItemSuperTypeEnum.SUPERTYPE_DOFUS,
                    new[]
                    {
                        CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_1,
                        CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_2,
                        CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_3,
                        CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_4,
                        CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_5,
                        CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_6
                    }
                },
                {ItemSuperTypeEnum.SUPERTYPE_SHIELD, new[] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD}},
                {ItemSuperTypeEnum.SUPERTYPE_BOOST, new[] {CharacterInventoryPositionEnum.INVENTORY_POSITION_BOOST_FOOD}},
            };

        public Inventory(Character owner)
        {
            Owner = owner;
            InitializeEvents();
        }

        public Character Owner
        {
            get;
            private set;
        }

        /// <summary>
        ///   Amount of kamas owned by this character.
        /// </summary>
        public override int Kamas
        {
            get { return Owner.Kamas; }
            protected set
            {
                Owner.Kamas = value;
            }
        }

        public BasePlayerItem this[int guid]
        {
            get
            {
                return TryGetItem(guid);
            }
        }

        public int Weight
        {
            get
            {
                var weight = Items.Values.Sum(entry => entry.Weight);

                if (Tokens != null)
                {
                    weight -= Tokens.Weight;
                }

                return weight > 0 ? weight : 0;
            }
        }

        public uint WeightTotal
        {
            get { return 1000; } // todo : manage weight properly
        }

        public uint WeaponCriticalHit
        {
            get
            {
                BasePlayerItem weapon;
                if ((weapon = TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON)) != null)
                {
                    return weapon.Template is WeaponTemplate
                               ? (uint) (weapon.Template as WeaponTemplate).CriticalHitBonus
                               : 0;
                }

                return 0;
            }
        }

        public BasePlayerItem Tokens
        {
            get;
            private set;
        }

        internal void LoadInventory()
        {
            var records = ItemManager.Instance.FindPlayerItems(Owner.Id);

            Items = records.Select(entry => ItemManager.Instance.LoadPlayerItem(Owner, entry)).ToDictionary(entry => entry.Guid);
            foreach (var item in this)
            {
                m_itemsByPosition[item.Position].Add(item);

                if (item.IsEquiped())
                    ApplyItemEffects(item, false);
            }

            foreach (var itemSet in GetEquipedItems().
                Where(entry => entry.Template.ItemSet != null).
                Select(entry => entry.Template.ItemSet).Distinct())
            {
                ApplyItemSetEffects(itemSet, CountItemSetEquiped(itemSet), true, false);
            }

            if (TokenTemplate != null && ActiveTokens && Owner.Account.Tokens > 0)
            {
                Tokens = ItemManager.Instance.CreatePlayerItem(Owner, TokenTemplate, Owner.Account.Tokens);
                Items.Add(Tokens.Guid, Tokens); // cannot stack
            }
        }

        private void UnLoadInventory()
        {
            Items.Clear();
            foreach (var item in m_itemsByPosition)
            {
                m_itemsByPosition[item.Key].Clear();
            }
        }

        public override void Save()
        {
            lock (Locker)
            {
                var database = WorldServer.Instance.DBAccessor.Database;
                foreach (var item in Items.Where(item => Tokens == null || item.Value != Tokens))
                {
                    if (item.Value.IsTemporarily)
                        continue;

                    if (item.Value.Record.IsNew)
                    {
                        database.Insert(item.Value.Record);
                        item.Value.Record.IsNew = false;
                    }
                    else if (item.Value.Record.IsDirty)
                    {
                        database.Update(item.Value.Record);
                    }
                }

                while (ItemsToDelete.Count > 0)
                {
                    var item = ItemsToDelete.Dequeue();

                    database.Delete(item.Record);
                }

                // update tokens amount
                if ((Tokens != null || Owner.Account.Tokens <= 0) &&
                    (Tokens == null || Owner.Account.Tokens == Tokens.Stack))
                    return;

                Owner.Account.Tokens = Tokens == null ? 0 : Tokens.Stack;
                IPCAccessor.Instance.Send(new UpdateAccountMessage(Owner.Account));
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            UnLoadInventory();
            TeardownEvents();
        }

        #endregion

        public override void SetKamas(int amount)
        {
            if (amount >= MaxInventoryKamas)
            {
                Kamas = MaxInventoryKamas;            
                //344	Vous avez atteint le seuil maximum de kamas dans votre inventaire.
                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 344);
            }

            base.SetKamas(amount);
        }

        public BasePlayerItem AddItem(ItemTemplate template, uint amount = 1)
        {
            var item = TryGetItem(template);

            if (item != null && !item.IsEquiped())
            {            
                if (!item.OnAddItem())
                    return null;

                StackItem(item, amount);
            }
            else
            {
                item = ItemManager.Instance.CreatePlayerItem(Owner, template, amount);

                return !item.OnAddItem() ? null : AddItem(item);
            }

            return item;
        }

        public override  bool RemoveItem(BasePlayerItem item, bool delete = true)
        {
            return item.OnRemoveItem() && base.RemoveItem(item, delete);
        }

        public void RefreshItemInstance(BasePlayerItem item)
        {
            if (!Items.ContainsKey(item.Guid))
                return;

            Items.Remove(item.Guid);

            var newInstance = ItemManager.Instance.RecreateItemInstance(item);
            Items.Add(newInstance.Guid, newInstance);

            RefreshItem(item);
        }

        public bool CanEquip(BasePlayerItem item, CharacterInventoryPositionEnum position, bool send = true)
        {
            if (Owner.IsInFight() && Owner.Fight.State != FightState.Placement)
                return false;

            if (position == CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
                return true;

            if (!GetItemPossiblePositions(item).Contains(position))
                return false;

            if (item.Template.Level > Owner.Level)
            {
                if (send)
                    BasicHandler.SendTextInformationMessage(Owner.Client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 3);

                return false;
            }

            var weapon = TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON);
            if (item.Template.Type.ItemType == ItemTypeEnum.SHIELD && weapon != null && weapon.Template.TwoHanded)
            {
                if (send)
                    BasicHandler.SendTextInformationMessage(Owner.Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 78);

                return false;
            }

            var shield = TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD);
            if (item.Template is WeaponTemplate && item.Template.TwoHanded && shield != null)
            {
                if (send)
                    BasicHandler.SendTextInformationMessage(Owner.Client,
                        TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 79);

                return false;
            }

            return true;
        }

        public CharacterInventoryPositionEnum[] GetItemPossiblePositions(BasePlayerItem item)
        {
            return !m_itemsPositioningRules.ContainsKey(item.Template.Type.SuperType) ? new[] { CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED } : m_itemsPositioningRules[item.Template.Type.SuperType];
        }

        public void MoveItem(BasePlayerItem item, CharacterInventoryPositionEnum position)
        {
            if (!HasItem(item))
                return;

            if (position == item.Position)
                return;

            var oldPosition = item.Position;

            BasePlayerItem equipedItem;
            if (position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED &&
                // check if an item is already on the desired position
                ((equipedItem = TryGetItem(position)) != null))
            {
                if (equipedItem.AllowFeeding)
                {
                    if (!equipedItem.Feed(item))
                        return;

                    RemoveItem(item);
                    return;
                }

                if (item.AllowDropping)
                {
                    if (!item.Drop(equipedItem))
                        return;

                    RemoveItem(item);
                    return;
                }

                // if there is one we move it to the inventory
                if (CanEquip(item, position, false))
                    MoveItem(equipedItem, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
            }

            if (!CanEquip(item, position))
                return;

            // second check
            if (!HasItem(item))
                return;

            if (position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
                UnEquipedDouble(item);

            if (item.Stack > 1) // if the item to move is stack we cut it
            {
                CutItem(item, item.Stack - 1);
                // now we have 2 stack : itemToMove, stack = 1
                //						 newitem, stack = itemToMove.Stack - 1
            }

            item.Position = position;

            BasePlayerItem stacktoitem;
            if (position == CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED &&
                IsStackable(item, out stacktoitem) && stacktoitem != null)
                // check if we must stack the moved item
            {

                NotifyItemMoved(item, oldPosition);
                StackItem(stacktoitem, item.Stack); // in all cases Stack = 1 else there is an error
                RemoveItem(item);
            }
            else // else we just move the item
            {
                NotifyItemMoved(item, oldPosition);
            }
        }

        public MerchantItem MoveToMerchantBag(BasePlayerItem item, uint quantity, uint price)
        {
            if (!HasItem(item))
                return null;

            if (quantity > item.Stack || quantity == 0)
                return null;

            if (item.IsLinked())
                return null;

            RemoveItem(item, quantity);

            var existingItem = Owner.MerchantBag.FirstOrDefault(x => x.MustStackWith(item));

            if (existingItem != null)
            {
                existingItem.Price = price;
                Owner.MerchantBag.StackItem(existingItem, quantity);

                return existingItem;
            }

            var merchantItem = ItemManager.Instance.CreateMerchantItem(item, quantity, price);
            Owner.MerchantBag.AddItem(merchantItem);

            return merchantItem;
        }

        private bool UnEquipedDouble(BasePlayerItem itemToEquip)
        {
            if (itemToEquip.Template.Type.ItemType == ItemTypeEnum.DOFUS)
            {
                var dofus = GetEquipedItems().FirstOrDefault(entry => entry.Guid != itemToEquip.Guid && entry.Template.Id == itemToEquip.Template.Id);

                if (dofus != null)
                {
                    MoveItem(dofus, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);

                    return true;
                }
            }

            if (itemToEquip.Template.Type.ItemType == ItemTypeEnum.RING)
            {
                // we can equip the same ring if it doesn't own to an item set
                var ring = GetEquipedItems().FirstOrDefault(entry => entry.Guid != itemToEquip.Guid && entry.Template.Id == itemToEquip.Template.Id && entry.Template.ItemSetId > 0);

                if (ring != null)
                {
                    MoveItem(ring, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);

                    return true;
                }
            }

            return false;
        }


        public void ChangeItemOwner(Character newOwner, BasePlayerItem item, uint amount)
        {
            if (!HasItem(item.Guid))
                return;

            if (amount > item.Stack)
                amount = item.Stack;

            // delete the item if there is no more stack else we unstack it
            if (amount >= item.Stack)
            {
                RemoveItem(item);
            }
            else
            {
                UnStackItem(item, amount);
            }

            var copy = ItemManager.Instance.CreatePlayerItem(newOwner, item, amount);
            newOwner.Inventory.AddItem(copy);
        }

        public void CheckItemsCriterias()
        {
            foreach (var equipedItem in GetEquipedItems().ToArray().Where(equipedItem => !equipedItem.AreConditionFilled(Owner)))
            {
                MoveItem(equipedItem, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
            }
        }

        public bool CanUseItem(BasePlayerItem item, bool send = true)
        {
            if (!HasItem(item.Guid) || !item.IsUsable())
                return false;

            if (Owner.IsInFight() && Owner.Fight.State != FightState.Placement)
                return false;

            if (!item.AreConditionFilled(Owner))
            {
                if (send)
                    BasicHandler.SendTextInformationMessage(Owner.Client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 19);
                return false;
            }

            if (item.Template.Level > Owner.Level)
            {
                if (send)
                    BasicHandler.SendTextInformationMessage(Owner.Client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 3);
                return false;
            }

            return true;
        }

        public void UseItem(BasePlayerItem item, uint amount = 1)
        {
            UseItem(item, amount, null, null);
        }        
        
        public void UseItem(BasePlayerItem item, Cell targetCell, uint amount = 1)
        {
            UseItem(item, amount, targetCell, null);
        }        
        
        public void UseItem(BasePlayerItem item, Character target, uint amount = 1)
        {
            UseItem(item, amount, null, target);
        }        
        
        public void UseItem(BasePlayerItem item, uint amount , Cell targetCell, Character target)
        {
            if (!CanUseItem(item))
                return;

            if (amount > item.Stack)
                amount = item.Stack;

            var removeAmount = item.UseItem(amount, targetCell, target);

            if (removeAmount > 0)
                RemoveItem(item, removeAmount);
        }

        /// <summary>
        /// Cut an item into two parts
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public BasePlayerItem CutItem(BasePlayerItem item, uint amount)
        {
            if (amount >= item.Stack)
                return item;

            UnStackItem(item, amount);

            var newitem = ItemManager.Instance.CreatePlayerItem(Owner, item, amount);

            Items.Add(newitem.Guid, newitem);

            NotifyItemAdded(newitem);

            return newitem;
        }

        private void ApplyItemEffects(BasePlayerItem item, bool send = true)
        {
            foreach (var handler in item.Effects.Select(effect => EffectManager.Instance.GetItemEffectHandler(effect, Owner, item)))
            {
                handler.Apply();
            }

            if (send)
                Owner.RefreshStats();
        }

        private void ApplyItemSetEffects(ItemSetTemplate itemSet, int count, bool apply, bool send = true)
        {
            var effects = itemSet.GetEffects(count);

            foreach (var handler in effects.Select(effect => EffectManager.Instance.GetItemEffectHandler(effect, Owner, itemSet, apply)))
            {
                handler.Apply();
            }

            if (send)
                Owner.RefreshStats();
        }

        protected override void DeleteItem(BasePlayerItem item)
        {
            if (item == Tokens)
                return;

            base.DeleteItem(item);
        }

        protected override void OnItemAdded(BasePlayerItem item)
        {
            m_itemsByPosition[item.Position].Add(item);

            if (item.IsEquiped())
                ApplyItemEffects(item);

            InventoryHandler.SendObjectAddedMessage(Owner.Client, item);
            InventoryHandler.SendInventoryWeightMessage(Owner.Client);

            base.OnItemAdded(item);
        }

        protected override void OnItemRemoved(BasePlayerItem item)
        {
            m_itemsByPosition[item.Position].Remove(item);

            if (item == Tokens)
                Tokens = null;

            // not equiped
            var wasEquiped = item.IsEquiped();
            item.Position = CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED;

            if (wasEquiped)
                ApplyItemEffects(item, item.Template.ItemSet == null);

            if (wasEquiped && item.Template.ItemSet != null)
            {
                var count = CountItemSetEquiped(item.Template.ItemSet);

                if (count >= 0)
                    ApplyItemSetEffects(item.Template.ItemSet, count + 1, false);
                if (count > 0)
                    ApplyItemSetEffects(item.Template.ItemSet, count, true);

                InventoryHandler.SendSetUpdateMessage(Owner.Client, item.Template.ItemSet);
            }

            InventoryHandler.SendObjectDeletedMessage(Owner.Client, item.Guid);
            InventoryHandler.SendInventoryWeightMessage(Owner.Client);

            if (wasEquiped)
                CheckItemsCriterias();

            if (wasEquiped && item.AppearanceId != 0)
                Owner.UpdateLook();

            base.OnItemRemoved(item);
        }

        private void OnItemMoved(BasePlayerItem  item, CharacterInventoryPositionEnum lastPosition)
        {
            m_itemsByPosition[lastPosition].Remove(item);
            m_itemsByPosition[item.Position].Add(item);

            var wasEquiped = lastPosition != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED;
            var isEquiped = item.IsEquiped();

            if (wasEquiped && !isEquiped ||
                !wasEquiped && isEquiped)
                ApplyItemEffects(item, false);

            if (!item.OnEquipItem(wasEquiped))
                return;

            if (item.Template.ItemSet != null && !(wasEquiped && isEquiped))
            {
                var count = CountItemSetEquiped(item.Template.ItemSet);

                if (count >= 0)
                    ApplyItemSetEffects(item.Template.ItemSet, count + (wasEquiped ? 1 : -1), false);
                if (count > 0)
                    ApplyItemSetEffects(item.Template.ItemSet, count, true, false);

                InventoryHandler.SendSetUpdateMessage(Owner.Client, item.Template.ItemSet);
            }

            if (lastPosition == CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED && !item.AreConditionFilled(Owner))
            {
                BasicHandler.SendTextInformationMessage(Owner.Client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 19);
                MoveItem(item, lastPosition);
            }

            InventoryHandler.SendObjectMovementMessage(Owner.Client, item);
            InventoryHandler.SendInventoryWeightMessage(Owner.Client);

            if (lastPosition != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
                CheckItemsCriterias();

            if ((isEquiped || wasEquiped) && item.AppearanceId != 0)
                Owner.UpdateLook();

            Owner.RefreshActor();
            Owner.RefreshStats();
        }

        protected override void OnItemStackChanged(BasePlayerItem item, int difference)
        {
            InventoryHandler.SendObjectQuantityMessage(Owner.Client, item);
            InventoryHandler.SendInventoryWeightMessage(Owner.Client);

            base.OnItemStackChanged(item, difference);
        }

        protected override void OnKamasAmountChanged(int amount)
        {
            InventoryHandler.SendKamasUpdateMessage(Owner.Client, amount);

            base.OnKamasAmountChanged(amount);
        }

        public void RefreshItem(BasePlayerItem item)
        {
            InventoryHandler.SendObjectModifiedMessage(Owner.Client, item);
        }

        public override bool IsStackable(BasePlayerItem item, out BasePlayerItem stackableWith)
        {
            BasePlayerItem stack;
            if (( stack = TryGetItem(item.Template, item.Effects, item.Position, item) ) != null)
            {
                stackableWith = stack;
                return true;
            }

            stackableWith = null;
            return false;
        }

        public BasePlayerItem TryGetItem(CharacterInventoryPositionEnum position)
        {
            return Items.Values.FirstOrDefault(entry => entry.Position == position);
        }

        public BasePlayerItem TryGetItem(ItemTemplate template, IEnumerable<EffectBase> effects, CharacterInventoryPositionEnum position, BasePlayerItem except)
        {
            var entries = from entry in Items.Values
                                              where entry != except && entry.Template.Id == template.Id && entry.Position == position && effects.CompareEnumerable(entry.Effects)
                                              select entry;

            return entries.FirstOrDefault();
        }

        public BasePlayerItem[] GetItems(CharacterInventoryPositionEnum position)
        {
            return Items.Values.Where(entry => entry.Position == position).ToArray();
        }

        public BasePlayerItem[] GetEquipedItems()
        {
            return (from entry in Items
                   where entry.Value.IsEquiped()
                   select entry.Value).ToArray();
        }

        public int CountItemSetEquiped(ItemSetTemplate itemSet)
        {
            return GetEquipedItems().Count(entry => itemSet.Items.Contains(entry.Template));
        }

        public BasePlayerItem[] GetItemSetEquipped(ItemSetTemplate itemSet)
        {
            return GetEquipedItems().Where(entry => itemSet.Items.Contains(entry.Template)).ToArray();
        }

        public EffectBase[] GetItemSetEffects(ItemSetTemplate itemSet)
        {
            return itemSet.GetEffects(CountItemSetEquiped(itemSet));
        }

        public short[] GetItemsSkins()
        {
            return GetEquipedItems().Where(entry => entry.Position != CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS && entry.AppearanceId != 0).Select(entry => (short)entry.AppearanceId).ToArray();
        }

        public short? GetPetSkin()
        {
            var pet = TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS);

            if (pet == null || pet.AppearanceId == 0)
                return null;

            return (short?) pet.AppearanceId;
        }

        #region Events

        private void InitializeEvents()
        {
            Owner.FightEnded += OnFightEnded;
        }
        private void TeardownEvents()
        {
            Owner.FightEnded -= OnFightEnded;
        }

        private void OnFightEnded(Character character, CharacterFighter fighter)
        {
            // update boosts
            foreach (var boost in GetItems(CharacterInventoryPositionEnum.INVENTORY_POSITION_BOOST_FOOD))
            {
                var effect = boost.Effects.OfType<EffectMinMax>().FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_RemainingFights);

                if (effect == null)
                    continue;

                effect.ValueMax--;

                if (effect.ValueMax <= 0)
                    RemoveItem(boost);
                else
                    RefreshItem(boost);
            }
        }

        #endregion
    }
}