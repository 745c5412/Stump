﻿using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace Stump.Server.WorldServer.Database.Items
{
    public class PlayerMerchantItemRelator
    {
        public static string FetchQuery = "SELECT * FROM characters_items_selled";

        /// <summary>
        /// Use string.Format
        /// </summary>
        public static string FetchByOwner = "SELECT * FROM characters_items_selled WHERE OwnerId={0}";
    }

    [TableName("characters_items_selled")]
    public class PlayerMerchantItemRecord : ItemRecord<PlayerMerchantItemRecord>, IAutoGeneratedRecord
    {
        private int m_ownerId;

        [Index]
        public int OwnerId
        {
            get { return m_ownerId; }
            set
            {
                m_ownerId = value;
                IsDirty = true;
            }
        }

        private uint m_price;

        public uint Price
        {
            get { return m_price; }
            set
            {
                m_price = value;
                IsDirty = true;
            }
        }

        private uint m_stackSold;
 
        public uint StackSold
        {
            get { return m_stackSold; }
            set
            {
                m_stackSold = value;
                IsDirty = true;
            }
        }
    }
}