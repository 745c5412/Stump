using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dapper;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Timers;
using Stump.DofusProtocol.Enums;
using Stump.ORM;
using Stump.Server.AuthServer.Database;
using Stump.Server.AuthServer.Database.Accounts;
using Stump.Server.AuthServer.Network;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.IPC.Messages;

namespace Stump.Server.AuthServer.Managers
{
    public class AccountManager : DataManager<AccountManager>
    {
        /// <summary>
        /// List of available breeds
        /// </summary>
        [Variable]
        public static List<PlayableBreedEnum> AvailableBreeds = new List<PlayableBreedEnum>
            {
            PlayableBreedEnum.Feca,
            PlayableBreedEnum.Osamodas,
            PlayableBreedEnum.Enutrof,
            PlayableBreedEnum.Sram,
            PlayableBreedEnum.Xelor,
            PlayableBreedEnum.Ecaflip,
            PlayableBreedEnum.Eniripsa,
            PlayableBreedEnum.Iop,
            PlayableBreedEnum.Cra,
            PlayableBreedEnum.Sadida,
            PlayableBreedEnum.Sacrieur,
            PlayableBreedEnum.Pandawa,
            PlayableBreedEnum.Roublard,
            PlayableBreedEnum.Zobal,
            PlayableBreedEnum.Steamer
            };


        [Variable]
        public static int CacheTimeout = 300;

        [Variable]
        public static int BansRefreshTime = 60;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<string, Tuple<DateTime, Account>> m_accountsCache = new Dictionary<string, Tuple<DateTime, Account>>();
        private List<IpBan> m_ipBans = new List<IpBan>();
        private List<ClientKeyBan> m_keyBans = new List<ClientKeyBan>(); 
        private SimpleTimerEntry m_timer;
        private SimpleTimerEntry m_bansTimer;

        public AccountManager()
        {
            
        }

        public override async void Initialize()
        {
            base.Initialize();
            m_timer = AuthServer.Instance.IOTaskPool.CallPeriodically(CacheTimeout * 60 / 4, TimerTick);
            m_bansTimer = AuthServer.Instance.IOTaskPool.CallPeriodically(BansRefreshTime * 1000, RefreshBans);
            m_ipBans = new List<IpBan>(await IpBan.Table.QueryAsync(IpBanRelator.FetchQuery));
            m_keyBans = new List<ClientKeyBan>(await ClientKeyBan.Table.QueryAsync(ClientKeyBanRelator.FetchQuery));
        }

        public override void TearDown()
        {
            AuthServer.Instance.IOTaskPool.CancelSimpleTimer(m_timer);
            AuthServer.Instance.IOTaskPool.CancelSimpleTimer(m_bansTimer);
        }

        private void TimerTick()
        {
            var toRemove = (from keyPair in m_accountsCache where keyPair.Value.Item1 <= DateTime.Now select keyPair).ToList();

            foreach (var keyPair in toRemove)
            {
                m_accountsCache.Remove(keyPair.Key);
                logger.Debug("Ticket {0} uncached (life time : {1} DateTime.Now={2})", keyPair.Key, keyPair.Value.Item1, DateTime.Now);
            }
        }

        private void RefreshBans()
        {
            lock (m_ipBans)
            {
                m_ipBans.Clear();
                m_keyBans.Clear();

                m_ipBans.AddRange(IpBan.Table.Query(IpBanRelator.FetchQuery));
                m_keyBans.AddRange(ClientKeyBan.Table.Query(ClientKeyBanRelator.FetchQuery));
            }
        }

        public void AddIPBan(IpBan ban)
        {
            lock (m_ipBans)
            {
                m_ipBans.Add(ban);
            }
        }

        public void AddClientKeyBan(ClientKeyBan ban)
        {
            lock (m_keyBans)
            {
                m_keyBans.Add(ban);
            }
        }

        public async Task<Account> FindAccountById(int id)
        {
            return await FindAccount(string.Format(AccountRelator.FindAccountById, id));
        }

        public async Task<Account> FindAccountByLogin(string login)
        {
            return await  FindAccount(string.Format(AccountRelator.FindAccountByLogin, login));
        }

        public async Task<Account> FindAccountByNickname(string nickname)
        {
            return await FindAccount(string.Format(AccountRelator.FindAccountByNickname, nickname));
        }

        public async Task<Account> FindAccountByCharacterId(int characterId)
        {
            return await FindAccount(string.Format(AccountRelator.FindAccountByCharacterId, characterId));
        }

        private static async Task<Account> FindAccount(string query)
        {
            return (await Account.Table.WrapExecution(con => con.QueryAsync<Account, WorldCharacter, Account>
                (
                    query,
                    new AccountRelator().Map
                ))).SingleOrDefault();
        }

        public IpBan FindIpBan(string ip)
        {
            lock (m_ipBans)
            {
                return m_ipBans.FirstOrDefault(x => x.IPAsString == ip);
            }
        }

        public ClientKeyBan FindClientKeyBan(string key)
        {
            lock (m_keyBans)
            {
                return m_keyBans.FirstOrDefault(x => x.ClientKey == key);
            }
        }

        public ClientKeyBan FindMatchingClientKeyBan(string key)
        {
            lock (m_keyBans)
            {
                var bans = m_keyBans.Where(entry => entry.ClientKey == key);

                return bans.OrderByDescending(entry => entry.GetRemainingTime()).FirstOrDefault();
            }
        }

        public IpBan FindMatchingIpBan(string ipStr)
        {
            lock (m_ipBans)
            {
                var ip = IPAddress.Parse(ipStr);
                var bans = m_ipBans.Where(entry => entry.Match(ip));

                return bans.OrderByDescending(entry => entry.GetRemainingTime()).FirstOrDefault();
            }
        }

        public async Task<UserGroupRecord> FindUserGroup(int id)
        {
            return (await UserGroupRecord.Table.QueryAsync(string.Format(UserGroupRelator.FindUserById, id))).SingleOrDefault();
        }

        public void CacheAccount(Account account)
        {
            if (m_accountsCache.ContainsKey(account.Ticket))
            {
                if (m_accountsCache[account.Ticket].Item2.Id != account.Id)
                {
                    throw new Exception("BE CAREFUL, two accounts have the same ticket");
                }

                m_accountsCache[account.Ticket] = Tuple.Create(DateTime.Now + TimeSpan.FromSeconds(CacheTimeout),
                    account);
            }
            else
                m_accountsCache.Add(account.Ticket,
                    Tuple.Create(DateTime.Now + TimeSpan.FromSeconds(CacheTimeout), account));
        }

        public void UnCacheAccount(Account account)
        {
            m_accountsCache.Remove(account.Ticket);
            logger.Debug("Uncache ticket {0}", account.Ticket);
        }

        public Account FindCachedAccountByTicket(string ticket)
        {
            Tuple<DateTime, Account> tuple;
            return m_accountsCache.TryGetValue(ticket, out tuple) ? tuple.Item2 : null;
        }

        public async Task<bool> LoginExists(string login)
        {
            return (await Account.Table.ExecuteAsync("SELECT EXISTS(SELECT 1 FROM accounts WHERE Login=@0)", login)) == 1;
        }

        public async Task<bool> NicknameExists(string nickname)
        {
            return await Account.Table.ExecuteAsync("SELECT EXISTS(SELECT 1 FROM accounts WHERE Nickname=@0)", nickname) == 1;
        }

        public async Task<bool> CreateAccount(Account account)
        {
            if (await LoginExists(account.Login))
                return false;

            await Account.Table.InsertAsync(account);
            return true;
        }

        public async Task<bool> DeleteAccount(Account account)
        {
            return await Account.Table.DeleteAsync(account);
        }

        public async Task<WorldCharacter> CreateAccountCharacter(Account account, WorldServer world, int characterId)
        {
            if (account.WorldCharacters.Any(entry => entry.CharacterId == characterId))
                return null;

            var character = new WorldCharacter
                                {
                                    AccountId = account.Id,
                                    WorldId = world.Id,
                                    CharacterId = characterId
                                };

            account.WorldCharacters.Add(character);

            // Try to insert the new world character
            if (!await WorldCharacter.Table.InsertAsync(character))
                return null;

            return character;
        }

        public async Task<bool> DeleteAccountCharacter(Account account, WorldServer world, int characterId)
        {
            var success = await Account.Table.ExecuteAsync(string.Format("DELETE FROM worlds_characters WHERE AccountId={0} AND CharacterId={1} AND WorldId={2}", account.Id, characterId, world.Id)) > 0;

            if (!success)
                return false;

            await CreateDeletedCharacter(account, world, characterId);
            account.WorldCharacters.RemoveAll(x => x.CharacterId == characterId && x.WorldId == world.Id);

            return true;
        }

        public bool AddAccountCharacter(Account account, WorldServer world, int characterId)
        {
            var character = CreateAccountCharacter(account, world, characterId);


            return true;
        }

        public async Task<WorldCharacterDeleted> CreateDeletedCharacter(Account account, WorldServer world, int characterId)
        {
            var character = new WorldCharacterDeleted
                                {
                                    AccountId = account.Id,
                                    WorldId = world.Id,
                                    CharacterId = characterId,
                                    DeletionDate = DateTime.Now
                                };

            await WorldCharacterDeleted.Table.InsertAsync(character);
            
            return character;
        }

        public async Task<bool> DeleteDeletedCharacter(WorldCharacterDeleted deletedCharacter)
        {
            if (deletedCharacter == null)
                return false;

            return await WorldCharacterDeleted.Table.DeleteAsync(deletedCharacter);
        }

        public void DisconnectClientsUsingAccount(Account account, AuthClient except = null)
        {
            DisconnectClientsUsingAccount(account, except, result => { }); // do nothing
        }

        public void DisconnectClientsUsingAccount(Account account, AuthClient except, Action<bool> callback)
        {
            var clients = AuthServer
                .Instance
                .FindClients
                (
                    entry => 
                    entry != except && 
                    entry.Account != null && 
                    entry.Account.Id == account.Id
                ).ToArray();

            // disconnect clients from auth server
            foreach (var client in clients)
            {
                client.Disconnect();
            }

            if (account.LastConnectionWorld == null)
            {
                callback(false);
                return;
            }

            var server = WorldServerManager.Instance.GetServerById(account.LastConnectionWorld.Value);

            if (server != null && server.Connected && server.IPCClient != null)
            {
                server.IPCClient.SendRequest<DisconnectedClientMessage>(new DisconnectClientMessage(account.Id),
                    msg => callback(msg.Disconnected), msg => callback(false));
            }
            else
            {
                callback(false);
            }
        }
    }
}