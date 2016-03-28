using System;

namespace Stump.Server.WorldServer.Game.Items
{
    public class ItemsStorage<T> : PersistantItemsCollection<T>
        where T : IPersistantItem
    {
        public event Action<ItemsStorage<T>, int> KamasAmountChanged;

        private void NotifyKamasAmountChanged(int kamas)
        {
            OnKamasAmountChanged(kamas);

            var handler = KamasAmountChanged;
            if (handler != null) handler(this, kamas);
        }

        protected virtual void OnKamasAmountChanged(int amount)
        {
        }

        public void AddKamas(int amount)
        {
            if (amount == 0)
                return;

            SetKamas(Kamas + amount);
        }

        public void SubKamas(int amount)
        {
            if (amount == 0)
                return;

            SetKamas(Kamas - amount);
        }

        public virtual void SetKamas(int amount)
        {
            var oldKamas = Kamas;

            if (amount < 0)
                amount = 0;

            Kamas = amount;
            NotifyKamasAmountChanged(amount - oldKamas);
        }

        public virtual int Kamas
        {
            get;
            protected set;
        }
    }
}