﻿using Stump.DofusProtocol.Enums.Custom;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Fights.Challenges;
using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.WorldServer.Handlers.Context
{
    public partial class ContextHandler
    {
        [WorldHandler(ChallengeTargetsListRequestMessage.Id)]
        public static void HandleChallengeTargetsListRequestMessage(WorldClient client, ChallengeTargetsListRequestMessage message)
        {
            if (!client.Character.IsFighting())
                return;

            var challenge = client.Character.Fight.Challenges.FirstOrDefault(x => x.Id == message.challengeId);

            if (challenge?.Target == null)
                return;

            if (!challenge.Target.IsVisibleFor(client.Character))
                return;

            SendChallengeTargetsListMessage(challenge.Fight.Clients, new[] { challenge.Target.Id }, new[] { challenge.Target.Cell.Id });
        }

        public static void SendChallengeInfoMessage(IPacketReceiver client, DefaultChallenge challenge)
        {
            client.Send(new ChallengeInfoMessage((short)challenge.Id, challenge.Target != null ? challenge.Target.Id : -1, challenge.Bonus, 0, challenge.Bonus, 0));
        }

        public static void SendChallengeResultMessage(IPacketReceiver client, DefaultChallenge challenge)
        {
            client.Send(new ChallengeResultMessage((short)challenge.Id, challenge.Status == ChallengeStatusEnum.SUCCESS));
        }

        public static void SendChallengeTargetsListMessage(IPacketReceiver client, IEnumerable<int> targetIds, IEnumerable<short> targetCells)
        {
            client.Send(new ChallengeTargetsListMessage(targetIds, targetCells));
        }
    }
}