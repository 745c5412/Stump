﻿using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Items.Player
{
    public class Bank : ItemsStorage<BankItem>
    {
        public Bank(Character character)
        {
            Owner = character;
            IsLoaded = false;
        }

        public void LoadRecord()
        {
            if (IsLoaded)
                return;

            WorldServer.Instance.IOTaskPool.EnsureContext();

            Items =
                WorldServer.Instance.DBAccessor.Database.Query<BankItemRecord>(string.Format(BankItemRelator.FetchByOwner,
                    Owner.Id)).ToDictionary(x => x.Id, x => new BankItem(Owner, x));
            IsLoaded = true;
        }

        public bool IsLoaded
        {
            get;
            private set;
        }

        public Character Owner
        {
            get;
            private set;
        }

        public override int Kamas
        {
            get { return Owner.Client.WorldAccount.BankKamas; }
            protected set { Owner.Client.WorldAccount.BankKamas = value; }
        }

        public bool StoreItem(BasePlayerItem item, uint amount)
        {
            if (!Owner.Inventory.HasItem(item))
                return false;

            if (amount > item.Stack)
                amount = item.Stack;

            var bankItem = TryGetItem(item.Template, item.Effects);

            if (bankItem != null)
            {
                bankItem.Stack += amount;
            }
            else
            {
                bankItem = ItemManager.Instance.CreateBankItem(Owner, item, amount);
                AddItem(bankItem);
            }

            Owner.Inventory.RemoveItem(item, amount);

            return true;
        }

        public bool StoreKamas(int kamas)
        {
            if (kamas < 0)
                return false;

            if (Owner.Inventory.Kamas < kamas)
                kamas = Owner.Inventory.Kamas;

            AddKamas(kamas);
            Owner.Inventory.SetKamas(Owner.Inventory.Kamas - kamas);


            return true;
        }

        public bool TakeItemBack(BankItem item, uint amount)
        {
            if (!HasItem(item))
                return false;

            if (amount > item.Stack)
                amount = item.Stack;

            var playerItem = Owner.Inventory.TryGetItem(item.Template, item.Effects,
                CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);

            if (playerItem != null)
            {
                playerItem.Stack += amount;
                Owner.Inventory.RefreshItem(playerItem);
            }
            else
            {
                playerItem = ItemManager.Instance.CreatePlayerItem(Owner, item.Template, amount, new List<EffectBase>(item.Effects));
                Owner.Inventory.AddItem(playerItem);
            }

            RemoveItem(item, amount);

            return true;
        }

        public bool TakeKamas(int kamas)
        {
            if (kamas < 0)
                return false;

            if (kamas > Kamas)
                kamas = Kamas;

            SubKamas(kamas);
            Owner.Inventory.AddKamas(kamas);
            return true;
        }


        protected override void OnItemAdded(BankItem item)
        {
            InventoryHandler.SendStorageObjectUpdateMessage(Owner.Client, item);

            base.OnItemAdded(item);
        }

        protected override void OnItemRemoved(BankItem item)
        {            
            InventoryHandler.SendStorageObjectRemoveMessage(Owner.Client, item);

            base.OnItemRemoved(item);
        }

        protected override void OnItemStackChanged(BankItem item, int difference)
        {            
            InventoryHandler.SendStorageObjectUpdateMessage(Owner.Client, item);

            base.OnItemStackChanged(item, difference);
        }

        protected override void OnKamasAmountChanged(int amount)
        {
            InventoryHandler.SendStorageKamasUpdateMessage(Owner.Client, amount);

            base.OnKamasAmountChanged(amount);
        }
    }
}