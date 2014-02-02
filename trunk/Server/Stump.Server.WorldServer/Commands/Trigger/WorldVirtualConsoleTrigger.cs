﻿using System;
using Stump.Core.IO;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Game;

namespace Stump.Server.WorldServer.Commands.Trigger
{
    class WorldVirtualConsoleTrigger : TriggerBase
    {
        public WorldVirtualConsoleTrigger(StringStream args)
            : base(args)
        {
        }

        public WorldVirtualConsoleTrigger(string args)
            : base(args)
        {
        }

        public WorldVirtualConsoleTrigger(StringStream args, Action<bool, string> callback)
            : base(args)
        {
            Callback = callback;
        }

        public Action<bool, String> Callback
        {
            get;
            private set;
        }

        public override bool CanFormat
        {
            get
            {
                return false;
            }
        }

        public override void Reply(string text)
        {
            if (Callback != null)
                Callback(true, text);
            else
                User.Commands.Add(text);
        }

        public override void ReplyError(string message)
        {
            if (Callback != null)
                Callback(false, "(Error) " + message);
            else
                Reply("(Error) " + message);
        }

        public override ICommandsUser User
        {
            get { return WorldServer.Instance.VirtualConsoleInterface; }
        }
    }
}
