using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.WorldServer.Handlers.Basic
{
    public class BasicHandler : WorldHandlerContainer
    {
        [WorldHandler(BasicSwitchModeRequestMessage.Id)]
        public static void HandleBasicSwitchModeRequestMessage(WorldClient client, BasicSwitchModeRequestMessage message)
        {
            client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, client.Character.ToggleAway() ? (short)37 : (short)38);
        }

        [WorldHandler(BasicWhoAmIRequestMessage.Id)]
        public static void HandleBasicWhoAmIRequestMessage(WorldClient client, BasicWhoAmIRequestMessage message)
        {
            /* Get Current character */
            var character = client.Character;

            /* Send informations about it */
            client.Send(new BasicWhoIsMessage(true, (sbyte)character.UserGroup.Role,
                                              character.Client.WorldAccount.Nickname, character.Name, (short)character.Map.SubArea.Id));
        }

        [WorldHandler(BasicWhoIsRequestMessage.Id)]
        public static void HandleBasicWhoIsRequestMessage(WorldClient client, BasicWhoIsRequestMessage message)
        {
            /* Get character */
            var character = World.Instance.GetCharacter(message.search);

            /* check null */
            if (character == null)
            {
                client.Send(new BasicWhoIsNoMatchMessage(message.search));
            }
            else
            {
                /* Send info about it */
                client.Send(new BasicWhoIsMessage(message.search == client.Character.Name,
                                                  (sbyte)character.UserGroup.Role,
                                                  character.Client.WorldAccount.Nickname, character.Name,
                                                  (short)character.Map.SubArea.Id));
            }
        }

        [WorldHandler(NumericWhoIsRequestMessage.Id)]
        public static void HandleNumericWhoIsRequestMessage(WorldClient client, NumericWhoIsRequestMessage message)
        {
            /* Get character */
            var character = World.Instance.GetCharacter(message.playerId);

            /* check null */
            if (character != null)
            {
                /* Send info about it */
                client.Send(new NumericWhoIsMessage(character.Id, character.Account.Id));
            }
        }

        public static void SendTextInformationMessage(IPacketReceiver client, TextInformationTypeEnum msgType, short msgId,
                                                      params string[] arguments)
        {
            client.Send(new TextInformationMessage((sbyte)msgType, msgId, arguments));
        }

        public static void SendTextInformationMessage(IPacketReceiver client, TextInformationTypeEnum msgType, short msgId,
                                                      params object[] arguments)
        {
            client.Send(new TextInformationMessage((sbyte)msgType, msgId, arguments.Select(entry => entry.ToString())));
        }

        public static void SendTextInformationMessage(IPacketReceiver client, TextInformationTypeEnum msgType, short msgId)
        {
            client.Send(new TextInformationMessage((sbyte)msgType, msgId, new string[0]));
        }

        public static void SendSystemMessageDisplayMessage(IPacketReceiver client, bool hangUp, short msgId, IEnumerable<string> arguments)
        {
            client.Send(new SystemMessageDisplayMessage(hangUp, msgId, arguments));
        }

        public static void SendSystemMessageDisplayMessage(IPacketReceiver client, bool hangUp, short msgId, params object[] arguments)
        {
            client.Send(new SystemMessageDisplayMessage(hangUp, msgId, arguments.Select(entry => entry.ToString())));
        }

        public static void SendBasicTimeMessage(IPacketReceiver client)
        {
            var offset = (short)TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalSeconds;
            client.Send(new BasicTimeMessage(DateTime.Now.GetUnixTimeStamp(), offset));
        }

        public static void SendBasicNoOperationMessage(IPacketReceiver client)
        {
            client.Send(new BasicNoOperationMessage());
        }

        public static void SendCinematicMessage(IPacketReceiver client, short cinematicId)
        {
            client.Send(new CinematicMessage(cinematicId));
        }
    }
}