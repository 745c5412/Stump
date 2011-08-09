using System;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Handlers.Characters;
using Stump.Server.WorldServer.Worlds.Breeds;

namespace Stump.Server.WorldServer.Handlers.Context.RolePlay
{
    public partial class ContextRoleplayHandler : WorldHandlerContainer
    {
        [WorldHandler(StatsUpgradeRequestMessage.Id)]
        public static void HandleStatsUpgradeRequestMessage(WorldClient client, StatsUpgradeRequestMessage message)
        {
            var statsid = (StatsBoostTypeEnum)message.statId;

            if (statsid < StatsBoostTypeEnum.Strength ||
                statsid > StatsBoostTypeEnum.Intelligence)
                throw new Exception("Wrong statsid");

            if (message.boostPoint <= 0)
                throw new Exception("Client given 0 as boostpoint. Forbidden value.");

            var breed = client.ActiveCharacter.Breed;
            uint neededpts = breed.GetNeededPointForStats(client.ActiveCharacter.Stats[statsid.ToString()].Base, statsid);

            var boost = (short) (message.boostPoint/ (double)neededpts);

            if (boost < 0)
                throw new Exception("Client is attempt to use more points that he has.");

            client.ActiveCharacter.Stats[statsid.ToString()].Base += boost;

            SendStatsUpgradeResultMessage(client, message.boostPoint);
            CharacterHandler.SendCharacterStatsListMessage(client);
        }

        public static void SendStatsUpgradeResultMessage(WorldClient client, short usedpts)
        {
            client.Send(new StatsUpgradeResultMessage(usedpts));
        }
    }
}