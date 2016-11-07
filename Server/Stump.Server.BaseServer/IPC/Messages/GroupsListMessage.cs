﻿using ProtoBuf;
using Stump.Server.BaseServer.IPC.Objects;
using System.Collections.Generic;

namespace Stump.Server.BaseServer.IPC.Messages
{
    [ProtoContract]
    public class GroupsListMessage : IPCMessage
    {
        public GroupsListMessage()
        {
        }

        public GroupsListMessage(List<UserGroupData> groups)
        {
            Groups = groups;
        }

        [ProtoMember(1)]
        public IList<UserGroupData> Groups
        {
            get;
            set;
        }
    }
}