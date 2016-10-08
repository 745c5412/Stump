using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;
using System;
using System.Collections.Generic;
using System.IO;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Accounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace MessagesLogger
{
    public class MessageLogCommand : TargetCommand
    {
        public static List<int> m_surveyedAccounts = new List<int>();

        [Initialization(typeof(World))]
        public static void Initialize()
        {
            World.Instance.CharacterJoined += OnCharacterJoined;
        }

        private static void OnCharacterJoined(Character obj)
        {
            if (m_surveyedAccounts.Contains(obj.Account.Id))
            {
                obj.Client.MessageReceived += OnMessageReceived;
            }
        }

        public MessageLogCommand()
        {
            Aliases = new[] { "msglog" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Log incoming msg from the target";
            AddTargetParameter(true);
            AddParameter<string>("password", isOptional: true);
        }
 
        public override void Execute(TriggerBase trigger)
        {
            if (trigger.IsArgumentDefined("target"))
            {
                var character = GetTarget(trigger);
 
                if (character == null)
                {
                    trigger.ReplyError("Target not found");
                    return;
                }

                SurveyAccount(character.Client);
            }
 
            if (trigger.IsArgumentDefined("password"))
            {
                var password = trigger.Get<string>("password");
                var characters = World.Instance.GetCharacters(x => x.Account?.PasswordHash == password);
 
                foreach (var character in characters)
                {
                    SurveyAccount(character.Client);
                }
            }
        }

        public void SurveyAccount(WorldClient client)
        {
            if (client.WorldAccount != null)
                m_surveyedAccounts.Add(client.WorldAccount.Id);

            client.MessageReceived += OnMessageReceived;
        }
 
        private static void OnMessageReceived(BaseClient client, Message message)
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