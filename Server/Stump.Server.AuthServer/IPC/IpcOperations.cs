#region License GNU GPL
// IPCOperations.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NLog;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.AuthServer.Database;
using Stump.Server.AuthServer.Database.Accounts;
using Stump.Server.AuthServer.Managers;
using Stump.Server.BaseServer.IPC;
using Stump.Server.BaseServer.IPC.Messages;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.AuthServer.IPC
{
    public class IPCOperations
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


        private readonly Dictionary<Type, Action<object, IPCMessage>> m_handlers = new Dictionary<Type, Action<object, IPCMessage>>();

        public IPCOperations(IPCClient ipcClient)
        {
            Client = ipcClient;

            InitializeHandlers();
            InitializeDatabase();
        }

        public IPCClient Client
        {
            get;
            private set;
        }

        public WorldServer WorldServer
        {
            get { return Client.Server; }
        }
        
        private AccountManager AccountManager
        {
            get;
            set;
        }

        private void InitializeHandlers()
        {
            foreach (var method in GetType().GetMethods(BindingFlags.Instance| BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (method.Name == "HandleMessage")
                    continue;

                var parameters = method.GetParameters();

                if (parameters.Length != 1 || !parameters[0].ParameterType.IsSubclassOf(typeof(IPCMessage)))
                    continue;

                m_handlers.Add(parameters[0].ParameterType, (Action<object, IPCMessage>)DynamicExtension.CreateDelegate(method, typeof(IPCMessage)));
            }
        }

        private void InitializeDatabase()
        {
            logger.Info("Opening Database connection for '{0}' server", WorldServer.Name);
            AccountManager = new AccountManager();
            AccountManager.Initialize();
        }

        public void HandleMessage(IPCMessage message)
        {
            Action<object, IPCMessage> handler;
            if (!m_handlers.TryGetValue(message.GetType(), out handler))
            {
                logger.Error("Received message {0} but no method handle it !", message.GetType());
                return;
            }

            handler(this, message);
        }

        private async void Handle(AccountRequestMessage message)
        {
            if (!string.IsNullOrEmpty(message.Ticket))
            {
                // no DB action here
                var account = AccountManager.Instance.FindCachedAccountByTicket(message.Ticket);
                if (account == null)
                {
                    Client.SendError(string.Format("Account not found with ticket {0}", message.Ticket), message);
                    return;
                }
                AccountManager.Instance.UnCacheAccount(account);

                Client.ReplyRequest(new AccountAnswerMessage(account.Serialize()), message);
            }
            else if (!string.IsNullOrEmpty(message.Nickname))
            {
                var account = await AccountManager.FindAccountByNickname(message.Nickname);

                if (account == null)
                {
                    Client.SendError(string.Format("Account not found with nickname {0}", message.Nickname), message);
                    return;
                }

                Client.ReplyRequest(new AccountAnswerMessage(account.Serialize()), message);
            }
            else if (!string.IsNullOrEmpty(message.Login))
            {
                var account = await AccountManager.FindAccountByLogin(message.Login);

                if (account == null)
                {
                    Client.SendError(string.Format("Account not found with login {0}", message.Login), message);
                    return;
                }

                Client.ReplyRequest(new AccountAnswerMessage(account.Serialize()), message);
            }
            else if (message.Id.HasValue)
            {
                var account = await AccountManager.FindAccountById(message.Id.Value);
                
                if (account == null)
                {
                    Client.SendError(string.Format("Account not found with id {0}", message.Id), message);
                    return;
                }

                Client.ReplyRequest(new AccountAnswerMessage(account.Serialize()), message);
            }
            else if (message.CharacterId.HasValue)
            {
                var account = await AccountManager.FindAccountByCharacterId(message.CharacterId.Value);
                
                if (account == null)
                {
                    Client.SendError(string.Format("Account not found with character id {0}", message.CharacterId), message);
                    return;
                }

                Client.ReplyRequest(new AccountAnswerMessage(account.Serialize()), message);

            }
            else
            {
                Client.SendError("Ticket, Nickname, Login, CharacterId and Id null or empty", message);
            }
        }

        private void Handle(ChangeStateMessage message)
        {
            WorldServerManager.Instance.ChangeWorldState(WorldServer, message.State);
            Client.ReplyRequest(new CommonOKMessage(), message);
        }

        private async void Handle(ServerUpdateMessage message)
        {
            if (WorldServer.CharsCount == message.CharsCount)
            {
                Client.ReplyRequest(new CommonOKMessage(), message);
                return;
            }

            WorldServer.CharsCount = message.CharsCount;

            if (WorldServer.CharsCount >= WorldServer.CharCapacity &&
                WorldServer.Status == ServerStatusEnum.ONLINE)
            {
                WorldServerManager.Instance.ChangeWorldState(WorldServer, ServerStatusEnum.FULL);
            }

            if (WorldServer.CharsCount < WorldServer.CharCapacity &&
                WorldServer.Status == ServerStatusEnum.FULL)
            {
                WorldServerManager.Instance.ChangeWorldState(WorldServer, ServerStatusEnum.ONLINE);
            }

            await WorldServer.Table.UpdateAsync(WorldServer);

            Client.ReplyRequest(new CommonOKMessage(), message);
        }

        private async void Handle(CreateAccountMessage message)
        {
            var accountData = message.Account;

            var account = new Account
            {
                Id = accountData.Id,
                Login = accountData.Login,
                PasswordHash = accountData.PasswordHash,
                Nickname = accountData.Nickname,
                UserGroupId = accountData.UserGroupId,
                AvailableBreeds = accountData.AvailableBreeds,
                Ticket = accountData.Ticket,
                SecretQuestion = accountData.SecretQuestion,
                SecretAnswer = accountData.SecretAnswer,
                Lang = accountData.Lang,
                Email = accountData.Email
            };

            if (await AccountManager.CreateAccount(account))
                Client.ReplyRequest(new CommonOKMessage(), message);
            else
                Client.SendError(string.Format("Login {0} already exists", accountData.Login), message);
        }

        private async void Handle(UpdateAccountMessage message)
        {
            var account = await AccountManager.FindAccountById(message.Account.Id);

            if (account == null)
            {
                Client.SendError(string.Format("Account {0} not found", message.Account.Id), message);
                return;
            }

            account.PasswordHash = message.Account.PasswordHash;
            account.SecretQuestion = message.Account.SecretQuestion;
            account.SecretAnswer = message.Account.SecretAnswer;
            account.UserGroupId = message.Account.UserGroupId;
            account.Tokens = message.Account.Tokens;
            account.LastClientKey = message.Account.LastClientKey;

            await Account.Table.UpdateAsync(account);

            Client.ReplyRequest(new CommonOKMessage(), message);
            //Client.ReplyRequest(new UpdateAccountMessage(account.Serialize()), message);
        }

        private async void Handle(DeleteAccountMessage message)
        {
            Account account;
            if (message.AccountId != null)
                account = await AccountManager.FindAccountById((int) message.AccountId);
            else if (!string.IsNullOrEmpty(message.AccountName))
                account = await AccountManager.FindAccountByLogin(message.AccountName);
            else
            {
                Client.SendError("AccoundId and AccountName are null or empty", message);
                return;
            }

            if (account == null)
            {
                Client.SendError(string.Format("Account {0}{1} not found", message.AccountId, message.AccountName), message);
                return;
            }

            AccountManager.Instance.DisconnectClientsUsingAccount(account);

            if (await AccountManager.DeleteAccount(account))
                Client.ReplyRequest(new CommonOKMessage(), message);
            else
                Client.SendError(string.Format("Cannot delete {0}", account.Login), message);
        }

        private async void Handle(AddCharacterMessage message)
        {
            var account = await AccountManager.FindAccountById(message.AccountId);

            if (account == null)
            {
                Client.SendError(string.Format("Account {0} not found", message.AccountId), message);
                return;
            }

            if (AccountManager.AddAccountCharacter(account, WorldServer, message.CharacterId))
                Client.ReplyRequest(new CommonOKMessage(), message);
            else
                Client.SendError(string.Format("Cannot add {0} character to {1} account", message.CharacterId, message.AccountId), message);
        }

        private async void Handle(DeleteCharacterMessage message)
        {
            var account = await AccountManager.FindAccountById(message.AccountId);

            if (account == null)
            {
                Client.SendError(string.Format("Account {0} not found", message.AccountId), message);
                return;
            }

            if (await AccountManager.DeleteAccountCharacter(account, WorldServer, message.CharacterId))
                Client.ReplyRequest(new CommonOKMessage(), message);
            else
                Client.SendError(string.Format("Cannot delete {0} character from {1} account", message.CharacterId, message.AccountId), message);
        }

        private async void Handle(BanAccountMessage message)
        {
            Account victimAccount;
            if (message.AccountId != null)
                victimAccount = await AccountManager.FindAccountById((int)message.AccountId);
            else if (!string.IsNullOrEmpty(message.AccountName))
                victimAccount = await AccountManager.FindAccountByLogin(message.AccountName);
            else
            {
                Client.SendError("AccoundId and AccountName are null or empty", message);
                return;
            }

            if (victimAccount == null)
            {
                Client.SendError(string.Format("Account {0}{1} not found", message.AccountId, message.AccountName), message);
                return;
            }

            victimAccount.IsBanned = !message.Jailed;
            victimAccount.IsJailed = message.Jailed;

            victimAccount.BanReason = message.BanReason;
            victimAccount.BanEndDate = message.BanEndDate;
            victimAccount.BannerAccountId = message.BannerAccountId;

            await Account.Table.UpdateAsync(victimAccount);

            Client.ReplyRequest(new CommonOKMessage(), message);
        }

        private async void Handle(UnBanAccountMessage message)
        {
            Account victimAccount;
            if (message.AccountId != null)
                victimAccount = await AccountManager.FindAccountById((int)message.AccountId);
            else if (!string.IsNullOrEmpty(message.AccountName))
                victimAccount = await AccountManager.FindAccountByLogin(message.AccountName);
            else
            {
                Client.SendError("AccoundId and AccountName are null or empty", message);
                return;
            }

            if (victimAccount == null)
            {
                Client.SendError(string.Format("Account {0}{1} not found", message.AccountId, message.AccountName), message);
                return;
            }

            victimAccount.IsBanned = false;
            victimAccount.IsJailed = false;
            victimAccount.BanEndDate = null;
            victimAccount.BanReason = null;
            victimAccount.BannerAccountId = null;

            await Account.Table.UpdateAsync(victimAccount);

            Client.ReplyRequest(new CommonOKMessage(), message);
        }

        private async void Handle(BanIPMessage message)
        {
            var ipBan = AccountManager.FindIpBan(message.IPRange);
            var ip = IPAddressRange.Parse(message.IPRange);
            if (ipBan != null)
            {
                ipBan.BanReason = message.BanReason;
                ipBan.BannedBy = message.BannerAccountId;
                ipBan.Duration = message.BanEndDate.HasValue ? (int?)(message.BanEndDate - DateTime.Now).Value.TotalMinutes : null;
                ipBan.Date = DateTime.Now;

                await IpBan.Table.UpdateAsync(ipBan);
            }
            else
            {
                var record = new IpBan
                {
                    IP = ip,
                    BanReason = message.BanReason,
                    BannedBy = message.BannerAccountId,
                    Duration = message.BanEndDate.HasValue ? (int?)( message.BanEndDate - DateTime.Now ).Value.TotalMinutes : null,
                    Date = DateTime.Now
                };

                await IpBan.Table.InsertAsync(record);
                AccountManager.Instance.AddIPBan(record);
            }

            Client.ReplyRequest(new CommonOKMessage(), message);
        }

        private async void Handle(UnBanIPMessage message)
        {
            var ipBan = AccountManager.FindIpBan(message.IPRange);
            if (ipBan == null)
            {
                Client.SendError(string.Format("IP ban {0} not found", message.IPRange), message);
            }
            else
            {
                await IpBan.Table.DeleteAsync(ipBan);
                Client.ReplyRequest(new CommonOKMessage(), message);
            }
        }

        private async void Handle(BanClientKeyMessage message)
        {
            var key = AccountManager.FindClientKeyBan(message.ClientKey);
            if (key != null)
            {
                key.BanReason = message.BanReason;
                key.BannedBy = message.BannerAccountId;
                key.Duration = message.BanEndDate.HasValue ? (int?)(message.BanEndDate - DateTime.Now).Value.TotalMinutes : null;
                key.Date = DateTime.Now;

               await ClientKeyBan.Table.UpdateAsync(key);
            }
            else
            {
                var record = new ClientKeyBan
                {
                    ClientKey = message.ClientKey,
                    BanReason = message.BanReason,
                    BannedBy = message.BannerAccountId,
                    Duration = message.BanEndDate.HasValue ? (int?)(message.BanEndDate - DateTime.Now).Value.TotalMinutes : null,
                    Date = DateTime.Now
                };

                await ClientKeyBan.Table.InsertAsync(record);
                AccountManager.Instance.AddClientKeyBan(record);
            }

            Client.ReplyRequest(new CommonOKMessage(), message);
        }

        private async void Handle(UnBanClientKeyMessage message)
        {
            var keyBan = AccountManager.FindClientKeyBan(message.ClientKey);
            if (keyBan == null)
            {
                Client.SendError(string.Format("ClientKey ban {0} not found", message.ClientKey), message);
            }
            else
            {
                await ClientKeyBan.Table.DeleteAsync(keyBan);
                Client.ReplyRequest(new CommonOKMessage(), message);
            }
        }

        private void Handle(BanClientKeyRequestMessage message)
        {
            var key = AccountManager.FindMatchingClientKeyBan(message.ClientKey);
            if (key != null && key.GetRemainingTime() > TimeSpan.Zero)
            {              
                Client.ReplyRequest(new BanClientKeyAnswerMessage(true, key.GetEndDate()), message);
                return;
            }

            Client.ReplyRequest(new BanClientKeyAnswerMessage(), message);
        }

        private async void Handle(GroupsRequestMessage message)
        {
            Client.ReplyRequest(
                new GroupsListMessage((await UserGroupRecord.Table.QueryAsync(UserGroupRelator.FetchQuery)).Select(x => x.GetGroupData()).ToList()),
                message);
        }

        public void Dispose()
        {
            m_handlers.Clear();
        }
    }
}