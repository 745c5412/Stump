﻿using Stump.DofusProtocol.Enums;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using Stump.Server.WorldServer.Database.Characters;
using System;

namespace Stump.Server.WorldServer.Database.Guilds
{
    public class GuildMemberRelator
    {
        public static string FetchQuery = "SELECT gm.*,ch.Id,ch.Name,ch.Experience,ch.Breed,ch.Sex,ch.AlignmentSide,ch.LastUsage,ch.PrestigeRank FROM guild_members gm LEFT JOIN characters ch ON ch.Id = gm.CharacterId";

        /// <summary>
        /// Use string.Format
        /// </summary>
        public static string FetchByGuildId = "SELECT gm.*,ch.Id,ch.Name,ch.Experience,ch.Breed,ch.Sex,ch.AlignmentSide,ch.LastUsage,ch.PrestigeRank FROM guild_members gm LEFT JOIN characters ON characters.Id = gm.CharacterId WHERE GuildId={0}";

        /// <summary>
        /// Use string.Format
        /// </summary>
        public static string FindByCharacterId = "SELECT gm.*,ch.Id,ch.Name,ch.Experience,ch.Breed,ch.Sex,ch.AlignmentSide,ch.LastUsage,ch.PrestigeRank FROM guild_members gm LEFT JOIN characters ON characters.Id = gm.CharacterId WHERE CharacterId={0}";

        private GuildMemberRecord m_current;

        public GuildMemberRecord Map(GuildMemberRecord record, CharacterRecord character)
        {
            if (record == null)
                return m_current;

            if (m_current != null && m_current.CharacterId == record.CharacterId)
            {
                m_current.Character = character;
                return null;
            }

            var previous = m_current;

            m_current = record;
            m_current.Character = character;
            return previous;
        }
    }

    [TableName("guild_members")]
    public class GuildMemberRecord : IAutoGeneratedRecord
    {
        [PrimaryKey("CharacterId", false)]
        public int CharacterId
        {
            get;
            set;
        }

        [Ignore]
        public CharacterRecord Character
        {
            get;
            set;
        }

        public int AccountId
        {
            get;
            set;
        }

        [Index]
        public int GuildId
        {
            get;
            set;
        }

        public short RankId
        {
            get;
            set;
        }

        public GuildRightsBitEnum Rights
        {
            get;
            set;
        }

        public long GivenExperience
        {
            get;
            set;
        }

        public byte GivenPercent
        {
            get;
            set;
        }

        public string Name
        {
            get
            {
                return Character.Name;
            }
        }

        public long Experience
        {
            get
            {
                return Character.Experience;
            }
        }

        public PlayableBreedEnum Breed
        {
            get
            {
                return Character.Breed;
            }
        }

        public SexTypeEnum Sex
        {
            get
            {
                return Character.Sex;
            }
        }

        public AlignmentSideEnum AlignementSide
        {
            get
            {
                return Character.AlignmentSide;
            }
        }

        public DateTime? LastConnection
        {
            get
            {
                return Character.LastUsage;
            }
        }

        public int PrestigeRank
        {
            get { return Character.PrestigeRank; }
        }
    }
}