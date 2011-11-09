using System.Collections.Generic;
using System.Linq;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Dialogs;
using Stump.Server.WorldServer.Worlds.Items;

namespace Stump.Server.WorldServer.Worlds.Exchanges
{
    public class PlayerTrader : ITrader, IDialoger
    {
        public event ItemMovedHandler ItemMoved;

        private void NotifyItemMoved(Item item, bool modified, int difference)
        {
            ItemMovedHandler handler = ItemMoved;
            if (handler != null)
                handler(this, item, modified, difference);
        }

        public event KamasChangedHandler KamasChanged;

        private void NotifyKamasChanged(uint kamasAmount)
        {
            KamasChangedHandler handler = KamasChanged;
            if (handler != null)
                handler(this, kamasAmount);
        }

        public event ReadyStatusChangedHandler ReadyStatusChanged;

        private void NotifyReadyStatusChanged(bool isready)
        {
            ReadyStatusChangedHandler handler = ReadyStatusChanged;
            if (handler != null)
                handler(this, isready);
        }


        private List<Item> m_items = new List<Item>();

        public PlayerTrader(Character character, PlayerTrade trade)
        {
            Character = character;
            Trade = trade;
        }

        public IDialog Dialog
        {
            get { return Trade; }
        }

        ITrade ITrader.Trade
        {
            get { return Trade; }
        }

        public PlayerTrade Trade
        {
            get;
            private set;
        }

        public RolePlayActor Actor
        {
            get { return Character; }
        }

        public Character Character
        {
            get;
            private set;
        }

        public IEnumerable<Item> Items
        {
            get { return m_items; }
        }

        public uint Kamas
        {
            get;
            private set;
        }

        public bool ReadyToApply
        {
            get;
            private set;
        }

        public void ToggleReady()
        {
            ToggleReady(!ReadyToApply);
        }

        public void ToggleReady(bool status)
        {
            if (status == ReadyToApply)
                return;

            ReadyToApply = status;

            NotifyReadyStatusChanged(ReadyToApply);
        }

        public bool MoveItem(int guid, int amount)
        {
            var playerItem = Character.Inventory[guid];
            var tradeItem = Items.Where(entry => entry.Guid == guid).SingleOrDefault();

            ToggleReady(false);

            if (playerItem == null)
                return false;

            if (tradeItem != null)
            {
                if (playerItem.Stack < tradeItem.Stack + amount || tradeItem.Stack + amount < 0)
                    return false;

                var currentStack = tradeItem.Stack;
                tradeItem.Stack += amount;

                if (tradeItem.Stack == 0)
                    m_items.Remove(tradeItem);

                NotifyItemMoved(tradeItem, true, tradeItem.Stack - currentStack);

                return true;
            }

            if (amount > playerItem.Stack || amount < 0)
                return false;

            tradeItem = new Item(playerItem, amount);

            m_items.Add(tradeItem);

            NotifyItemMoved(tradeItem, false, amount);

            return true;
        }

        public bool SetKamas(uint amount)
        {
            ToggleReady(false);

            if (amount > Character.Inventory.Kamas)
                return false;

            Kamas = amount;

            NotifyKamasChanged(Kamas);

            return true;
        }
    }
}