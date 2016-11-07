﻿using Stump.Core.Collections;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Effects.Instances;
using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Items.BidHouse
{
    public class BidHouseCategory
    {
        public BidHouseCategory(int id, BidHouseItem item)
        {
            Id = id;
            ItemType = (ItemTypeEnum)item.Template.TypeId;
            TemplateId = item.Template.Id;
            Effects = item.Effects;
            ItemLevel = (int)item.Template.Level;
            Items = new ConcurrentList<BidHouseItem>();
        }

        public int Id
        {
            get;
        }

        public int TemplateId
        {
            get;
        }

        public ItemTypeEnum ItemType
        {
            get;
        }

        public int ItemLevel
        {
            get;
        }

        public List<EffectBase> Effects
        {
            get;
        }

        public ConcurrentList<BidHouseItem> Items
        {
            get;
        }

        #region Functions

        public List<BidHouseItem> GetItems()
        {
            var items = new List<BidHouseItem>();

            foreach (var quantity in BidHouseManager.Quantities)
            {
                var item = Items.OrderBy(x => x.Price).FirstOrDefault(x => x.Stack == quantity && !x.Sold);
                if (item == null)
                    continue;

                items.Add(item);
            }

            return items;
        }

        public IEnumerable<int> GetPrices()
        {
            var prices = new List<int>();

            foreach (var item in BidHouseManager.Quantities.Select(quantity => Items.Where(x => x.Stack == quantity && !x.Sold)
                .OrderBy(x => x.Price).FirstOrDefault()))
            {
                prices.Add(item != null ? (int)item.Price : 0);
            }

            return prices;
        }

        public BidHouseItem GetItem(int quantity, int price) => Items.FirstOrDefault(x => x.Stack == quantity && x.Price == price && !x.Sold);

        public bool IsValidForThisCategory(BidHouseItem item) => item.Template.Id == TemplateId && Effects.CompareEnumerable(item.Effects);

        public bool IsEmpty() => !Items.Any();

        #endregion Functions

        #region Network

        public BidExchangerObjectInfo GetBidExchangerObjectInfo()
        {
            return new BidExchangerObjectInfo(Id, 0, false, Effects.Select(x => x.GetObjectEffect()), GetPrices());
        }

        #endregion Network
    }
}