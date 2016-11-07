﻿using Stump.DofusProtocol.Enums;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace Stump.Server.WorldServer.Database.Items
{
    public class PlayerItemRelator
    {
        public static string FetchQuery = "SELECT * FROM characters_items";

        /// <summary>
        /// Use string.Format
        /// </summary>
        public static string FetchByOwner = "SELECT * FROM characters_items WHERE OwnerId={0}";
    }

    [TableName("characters_items")]
    public class PlayerItemRecord : ItemRecord<PlayerItemRecord>, IAutoGeneratedRecord
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

        private CharacterInventoryPositionEnum m_position;

        public CharacterInventoryPositionEnum Position
        {
            get { return m_position; }
            set
            {
                m_position = value;
                IsDirty = true;
            }
        }
    }
}