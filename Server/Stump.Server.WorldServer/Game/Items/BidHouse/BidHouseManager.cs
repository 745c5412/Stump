﻿using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Collections;
using Stump.Core.Extensions;
using Stump.Core.Pool;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.Items.BidHouse;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Items.BidHouse
{
    public class BidHouseManager : DataManager<BidHouseManager>, ISaveable
    {
        #region Fields

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private UniqueIdProvider m_idProvider;

        private ConcurrentList<BidHouseItem> m_bidHouseItems = new ConcurrentList<BidHouseItem>();
        private ConcurrentList<BidHouseCategory> m_bidHouseCategories = new ConcurrentList<BidHouseCategory>();

        public static int UnsoldDelay = 672;

        [Variable]
        public static float TaxPercent = 2;

        [Variable]
        public static IEnumerable<int> Quantities = new[] { 1, 10, 100 };

        #endregion

        #region Creators

        public BidHouseItem CreateBidHouseItem(Character character, BasePlayerItem item, int amount, uint price)
        {
            if (amount < 0)
                throw new ArgumentException("amount < 0", "amount");


            var guid = BidHouseItemRecord.PopNextId();
            var record = new BidHouseItemRecord // create the associated record
            {
                Id = guid,
                OwnerId = character.Account.Id,
                Price = price,
                SellDate = DateTime.Now,
                Template = item.Template,
                Stack = (uint)amount,
                Effects = new List<EffectBase>(item.Effects),
                IsNew = true
            };

            return new BidHouseItem(record);
        }

        #endregion

        #region Loading

        [Initialization(typeof(ItemManager))]
        public override void Initialize()
        {
            m_idProvider = new UniqueIdProvider(1);
            m_bidHouseItems = new ConcurrentList<BidHouseItem>(Database.Query<BidHouseItemRecord>(BidHouseItemRelator.FetchQuery).Select(x => new BidHouseItem(x)));

            foreach (var item in m_bidHouseItems)
            {
                var category = GetBidHouseCategory(item);

                if (category == null)
                {
                    category = new BidHouseCategory(m_idProvider.Pop(), item);
                    m_bidHouseCategories.Add(category);
                }

                category.Items.Add(item);
            }

            World.Instance.RegisterSaveableInstance(this);
        }

        #endregion

        #region Getters

        public BidHouseCategory GetBidHouseCategory(BidHouseItem item)
        {
            return m_bidHouseCategories.FirstOrDefault(x => x.IsValidForThisCategory(item));
        }

        public BidHouseCategory GetBidHouseCategory(int categoryId)
        {
            return m_bidHouseCategories.FirstOrDefault(x => x.Id == categoryId);
        }

        public List<BidHouseCategory> GetBidHouseCategories(int itemId)
        {
            return m_bidHouseCategories.Where(x => x.TemplateId == itemId).ToList();
        }

        public List<BidHouseItem> GetBidHouseItems(ItemTypeEnum type, int maxItemLevel)
        {
            return m_bidHouseItems.Where(x => x.Template.TypeId == (int)type && x.Template.Level <= maxItemLevel)
                .GroupBy(x => x.Template.Id).Select(x => x.First()).ToList();
        }

        public List<BidHouseItem> GetBidHouseItems(int ownerId)
        {
            return m_bidHouseItems.Where(x => x.Record.OwnerId == ownerId).ToList();
        }

        public BidHouseItem GetBidHouseItem(int guid)
        {
            return m_bidHouseItems.FirstOrDefault(x => x.Guid == guid);
        }

        public BidHouseItem GetBidHouseItem(int itemId, int quantity, int price)
        {
            return m_bidHouseItems.FirstOrDefault(x => x.Template.Id == itemId && x.Stack == quantity && x.Price == price);
        }

        public int GetAveragePriceForItem(int itemId)
        {
            var items = m_bidHouseItems.Where(x => x.Template.Id == itemId).Select(x => (int)(x.Price / x.Stack)).ToArray();

            if (!items.Any())
                return 0;

            return (int)Math.Round(items.Average());
        }

        #endregion

        #region Functions

        public event Action<BidHouseItem, BidHouseCategory, bool> ItemAdded;

        public void AddBidHouseItem(BidHouseItem item)
        {
            m_bidHouseItems.Add(item);

            var category = GetBidHouseCategory(item);
            var newCategory = false;

            if (category == null)
            {
                category = new BidHouseCategory(m_idProvider.Pop(), item);
                m_bidHouseCategories.Add(category);

                newCategory = true;
            }

            category.Items.Add(item);

            var handler = ItemAdded;

            if (handler != null)
                handler(item, category, newCategory);
        }

        public event Action<BidHouseItem, BidHouseCategory, bool> ItemRemoved;

        public void RemoveBidHouseItem(BidHouseItem item)
        {
            WorldServer.Instance.IOTaskPool.AddMessage(
                () => Database.Delete(item.Record));

            m_bidHouseItems.Remove(item);

            var category = GetBidHouseCategory(item);
            var categoryDeleted = false;

            category.Items.Remove(item);

            if (category.IsEmpty())
            {
                m_bidHouseCategories.Remove(category);
                categoryDeleted = true;
            }

            var handler = ItemRemoved;

            if (handler != null)
                handler(item, category, categoryDeleted);
        }

        #endregion

        public void Save()
        {
            foreach (var item in m_bidHouseItems.Where(item => item.Record.IsDirty))
            {
                item.Save(Database);
            }
        }
    }

    public class BidHouseItemComparer : IEqualityComparer<BidHouseItem>
    {
        public bool Equals(BidHouseItem x, BidHouseItem y)
        {
            return x.Effects.CompareEnumerable(y.Effects);
        }

        public int GetHashCode(BidHouseItem obj)
        {
            return 0;
        }
    }
}
