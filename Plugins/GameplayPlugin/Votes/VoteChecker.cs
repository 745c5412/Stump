using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.BaseServer.IPC.Messages;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;
using System;

namespace GameplayPlugin.Votes
{
    public static class VoteChecker
    {
        [Variable]
        private static int VoteTimer = 300;

        private static DateTime VoteDateTime => (DateTime.Now - TimeSpan.FromHours(3));

        [Initialization(InitializationPass.Last, Silent = true)]
        public static void Initialize()
        {
            WorldServer.Instance.IOTaskPool.CallPeriodically((VoteTimer * 1000), CheckVotes);
        }

        private static void CheckVotes()
        {
            foreach (var character in World.Instance.GetCharacters(character => character.UserGroup.Role == RoleEnum.Player
                && (character.Account.LastVote == null || character.Account.LastVote < VoteDateTime)))
            {
                IPCAccessor.Instance.SendRequest<AccountAnswerMessage>(new AccountRequestMessage { Id = character.Account.Id },
                    msg => WorldServer.Instance.IOTaskPool.AddMessage(() => OnAccountReceived(msg, character.Client)));
            }
        }

        private static void OnAccountReceived(AccountAnswerMessage message, WorldClient client)
        {
            var character = client.Character;

            if (character == null)
                return;

            client.Account.LastVote = message.Account.LastVote;

            client.Character.DisplayNotification(
                "Plus de 3H se sont écoulées depuis votre dernier vote, vous pouvez à nouveau voter pour gagner des Ogrines en cliquant <u><b><a href='http://www.azote.us/vote' target='_blank'><font color='#0000FF'>ICI</font></a></b></u>",
                NotificationEnum.ERREUR);
        }
    }
}