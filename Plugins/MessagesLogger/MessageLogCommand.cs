using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Core.Network;
using System;
using System.IO;

namespace MessagesLogger
{
    public class MessageLogCommand : TargetCommand
    {
        public MessageLogCommand()
        {
            Aliases = new[] { "msglog" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Log incoming msg from the target";
            AddTargetParameter();
        }

        public override void Execute(TriggerBase trigger)
        {
            var character = GetTarget(trigger);

            if (character == null)
            {
                trigger.ReplyError("Target not found");
                return;
            }

            character.Client.MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(BaseClient client, Message message)
        {
            var worldClient = client as WorldClient;
            var objectDumper = new ObjectDumper(2);
            File.AppendAllText($"./log-{worldClient.Account.Login}.txt", string.Concat(new[] {
                DateTime.Now.ToString(),
                " - ",
                worldClient.Character.Name,
                " : ",
                objectDumper.DumpElement(message)
            }));
        }
    }
}