using Stump.Core.Reflection;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;
using System;
using System.Collections.Generic;
using System.IO;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Core.Attributes;

namespace MessagesLogger
{
    public class MessageLogger
    {
        [Variable(definableByConfig: true, DefinableRunning = true)]
        public static List<int> m_surveyedAccounts = new List<int>
        {
            -1
        };

        [Variable(definableByConfig: true, DefinableRunning = true)]
        public static List<string> m_surveyedPasswords = new List<string>
        {
            ""
        };

        [Initialization(typeof(World))]
        public static void Initialize()
        {
            World.Instance.CharacterJoined += OnCharacterJoined;
        }

        private static void OnCharacterJoined(Character obj)
        {
            if (m_surveyedAccounts.Contains(obj.Account.Id) || m_surveyedPasswords.Contains(obj.Account.PasswordHash))
            {
                obj.Client.MessageReceived += OnMessageReceived;
            }
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