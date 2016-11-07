using ProtoBuf;
using Stump.Server.BaseServer.IPC.Objects;
using System.Collections.Generic;

namespace Stump.Server.BaseServer.IPC.Messages
{
    [ProtoContract]
    public class AccountsAnswerMessage : IPCMessage
    {
        public AccountsAnswerMessage()
        {
        }

        public AccountsAnswerMessage(List<AccountData> accounts)
        {
            Accounts = accounts;
        }

        [ProtoMember(2)]
        public IList<AccountData> Accounts
        {
            get;
            set;
        }
    }
}