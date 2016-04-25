﻿using System.Text.RegularExpressions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.AuthServer.Managers;
using Stump.Server.AuthServer.Network;

namespace Stump.Server.AuthServer.Handlers.Connection
{
    public partial class ConnectionHandler
    {

        [AuthHandler(NicknameChoiceRequestMessage.Id)]
        public static async void HandleNicknameChoiceRequestMessage(AuthClient client, NicknameChoiceRequestMessage message)
        {
            var nickname = message.nickname;

            /* Check the Username */
            if (!CheckNickName(nickname))
            {
                client.Send(new NicknameRefusedMessage((sbyte) NicknameErrorEnum.INVALID_NICK));
                return;
            }

            /* Same as Login */
            if (nickname == client.Account.Login)
            {
                client.Send(new NicknameRefusedMessage((sbyte) NicknameErrorEnum.SAME_AS_LOGIN));
                return;
            }

            /* Look like Login */
            if (client.Account.Login.Contains(nickname))
            {
                client.Send(new NicknameRefusedMessage((sbyte) NicknameErrorEnum.TOO_SIMILAR_TO_LOGIN));
                return;
            }

            /* Already Used */
            if (await AccountManager.Instance.NicknameExists(nickname))
            {
                client.Send(new NicknameRefusedMessage((sbyte) NicknameErrorEnum.ALREADY_USED));
                return;
            }

            /* Ok, it's good */
            client.Account.Nickname = nickname;
            client.Save();

            client.Send(new NicknameAcceptedMessage());
            SendIdentificationSuccessMessage(client, false);
            SendServersListMessage(client);
        }

        public static bool CheckNickName(string nickName)
        {
            return Regex.IsMatch(nickName, @"^[a-zA-Z\-]{3,29}$", RegexOptions.Compiled);
        }

    }
}