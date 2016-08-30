using ProtoBuf;
using System;

namespace Stump.Server.BaseServer.IPC.Messages
{
    [ProtoContract]
    public class BanClientKeyAnswerMessage : IPCMessage
    {
        public BanClientKeyAnswerMessage()
        {
        }

        public BanClientKeyAnswerMessage(bool banned, DateTime endDate)
        {
            IsBanned = banned;
            EndDate = endDate;
        }

        [ProtoMember(2)]
        public bool IsBanned
        {
            get;
            set;
        }

        [ProtoMember(3)]
        public DateTime EndDate
        {
            get;
            set;
        }
    }
}