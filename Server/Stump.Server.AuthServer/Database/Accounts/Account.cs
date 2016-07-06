﻿using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using Stump.Server.BaseServer.IPC.Objects;

namespace Stump.Server.AuthServer.Database
{
    public class AccountRelator
    {
        public static string FetchQuery = "SELECT * FROM accounts LEFT JOIN worlds_characters ON worlds_characters.AccountId = accounts.Id";
        /// <summary>
        /// Use string.Format
        /// </summary>
        public static string FindAccountById = "SELECT * FROM accounts LEFT JOIN worlds_characters ON worlds_characters.AccountId = accounts.Id WHERE accounts.Id = {0}";
        /// <summary>
        /// Use SQL parameter
        /// </summary>
        public static string FindAccountByLogin = "SELECT * FROM accounts LEFT JOIN worlds_characters ON worlds_characters.AccountId = accounts.Id WHERE accounts.Login = @0";
        /// <summary>
        /// Use SQL parameter
        /// </summary>
        public static string FindAccountByNickname = "SELECT * FROM accounts LEFT JOIN worlds_characters ON worlds_characters.AccountId = accounts.Id WHERE accounts.Nickname = @0";
                /// <summary>
        /// Use string.Format
        /// </summary>
        public static string FindAccountByCharacterId = "SELECT * FROM accounts LEFT JOIN worlds_characters ON worlds_characters.AccountId = accounts.Id WHERE worlds_characters.CharacterId = {0}";



        private Account m_current;
        public Account Map(Account account, WorldCharacter character)
        {
            if (account == null)
                return m_current;

            if (m_current != null && m_current.Id == account.Id)
            {
                if (character.AccountId == account.Id)
                    m_current.WorldCharacters.Add(character);
                return null;
            }

            var previous = m_current;

            m_current = account;
            if (character.AccountId == account.Id)
                m_current.WorldCharacters.Add(character);

            return previous;
        }
    }

    [TableName("accounts")]
    public class Account : IAutoGeneratedRecord
    {
        private DateTime? m_loadedLastConnection;
        private string m_loadedLastConnectionIP;
        private List<PlayableBreedEnum> m_availableBreeds;

        #region Record Properties

        private long m_availableBreedsFlag;

        public Account()
        {
            AvailableBreedsFlag = 16383;
            WorldCharacters = new List<WorldCharacter>();
        }

        // Primitive properties
       
        [PrimaryKey("Id")]
        public int Id
        {
            get;
            set;
        }

        public string Login
        {
            get;
            set;
        }

        public string PasswordHash
        {
            get;
            set;
        }

        public string Nickname
        {
            get;
            set;
        }

        public int UserGroupId
        {
            get;
            set;
        }

        [Ignore]
        public long AvailableBreedsFlag
        {
            get { return m_availableBreedsFlag; }
            set
            {
                m_availableBreedsFlag = value;
                m_availableBreeds = new List<PlayableBreedEnum>();
                m_availableBreeds.AddRange(Enum.GetValues(typeof (PlayableBreedEnum)).Cast<PlayableBreedEnum>().
                                               Where(IsBreedAvailable));
            }
        }

        [NullString]
        public string Ticket
        {
            get;
            set;
        }

        public string SecretQuestion
        {
            get;
            set;
        }

        public string SecretAnswer
        {
            get;
            set;
        }

        public string Lang
        {
            get;
            set;
        }

        [NullString]
        public string Email
        {
            get;
            set;
        }

        public DateTime CreationDate
        {
            get;
            set;
        }

        public DateTime? LastVote
        {
            get;
            set;
        }

        private DateTime? m_lastConnection;

        public DateTime? LastConnection
        {
            get { return m_lastConnection; }
            set
            {
                m_lastConnection = value;

                if (m_loadedLastConnection == null)
                    m_loadedLastConnection = value;
            }
        }

        private string m_lastConnectedIp;

        [NullString]
        public string LastConnectedIp
        {
            get { return m_lastConnectedIp; }
            set
            {
                m_lastConnectedIp = value;

                if (m_loadedLastConnectionIP == null)
                    m_loadedLastConnectionIP = value;
            }
        }

        [NullString]
        public string LastClientKey
        {
            get;
            set;
        }

        public int? LastConnectionWorld
        {
            get;
            set;
        }

        public DateTime SubscriptionEnd
        {
            get;
            set;
        }

        public bool IsJailed
        {
            get;
            set;
        }

        public bool IsBanned
        {
            get;
            set;
        }

        public bool IsLifeBanned
        {
            get
            {
                return BanEndDate == null && IsBanned;
            }
        }

        [NullString]
        public string BanReason
        {
            get;
            set;
        }

        public DateTime? BanEndDate
        {
            get;
            set;
        }

        public int? BannerAccountId
        {
            get;
            set;
        }

        [Ignore]
        public List<WorldCharacter> WorldCharacters
        {
            get;
            set;
        }


        #endregion

        [Ignore]
        public List<PlayableBreedEnum> AvailableBreeds
        {
            get
            {
                if (m_availableBreeds != null)
                    return m_availableBreeds;

                m_availableBreeds = new List<PlayableBreedEnum>();
                m_availableBreeds.AddRange(Enum.GetValues(typeof (PlayableBreedEnum)).Cast<PlayableBreedEnum>().
                    Where(IsBreedAvailable));

                return m_availableBreeds;
            }
            set
            {
                m_availableBreeds = value;
                m_availableBreedsFlag =
                    (uint) value.Aggregate(0, (current, breedEnum) => current | (1 << ((int) breedEnum - 1)));
            }
        }

        public AccountData Serialize()
        {
            return new AccountData
                       {
                           Id = Id,
                           Login = Login,
                           PasswordHash = PasswordHash,
                           Nickname = Nickname,
                           UserGroupId = UserGroupId,
                           AvailableBreeds = AvailableBreeds,
                           Ticket = Ticket,
                           SecretQuestion = SecretQuestion,
                           SecretAnswer = SecretAnswer,
                           Lang = Lang,
                           Email = Email,
                           CreationDate = CreationDate,
                           IsJailed = IsJailed,
                           IsBanned = IsBanned,
                           BanEndDate = BanEndDate,
                           BanReason = BanReason,
                           LastConnection = m_loadedLastConnection,
                           LastConnectionIp = m_loadedLastConnectionIP,
                           LastClientKey = LastClientKey,
                           SubscriptionEndDate = SubscriptionEnd,
                           Characters = WorldCharacters.Select(entry => new WorldCharacterData(entry.CharacterId, entry.WorldId)).ToList(),
                           LastVote = LastVote,
                       };
        }

        public bool IsBreedAvailable(PlayableBreedEnum breed)
        {
            return true;

            if ((int) breed <= 0)
                return false;

            var flag = (1 << ((int) breed - 1));
            return (AvailableBreedsFlag & flag) == flag;
        }

        public sbyte GetCharactersCountByWorld(int worldId)
        {
            return (sbyte) WorldCharacters.Count(entry => entry.WorldId == worldId);
        }

        public IEnumerable<int> GetWorldCharactersId(int worldId)
        {
            return WorldCharacters.Where(c => c.WorldId == worldId).Select(c => c.CharacterId);
        }
    }
}