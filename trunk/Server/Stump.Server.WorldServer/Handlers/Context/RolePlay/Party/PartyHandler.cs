using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Worlds;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Parties;
using WorldParty = Stump.Server.WorldServer.Worlds.Parties.Party;

namespace Stump.Server.WorldServer.Handlers.Context.RolePlay.Party
{
    public class PartyHandler : WorldHandlerContainer
    {
        [WorldHandler(PartyInvitationRequestMessage.Id)]
        public static void HandlePartyInvitationRequestMessage(WorldClient client, PartyInvitationRequestMessage message)
        {
            var target = World.Instance.GetCharacter(message.name);

            if (target == null)
            {
                SendPartyCannotJoinErrorMessage(client, PartyJoinErrorEnum.PARTY_JOIN_ERROR_PLAYER_NOT_FOUND);
                return;
            }

            client.ActiveCharacter.Invite(target);
        }

        [WorldHandler(PartyInvitationDetailsRequestMessage.Id)]
        public static void HandlePartyInvitationDetailsRequestMessage(WorldClient client, PartyInvitationDetailsRequestMessage message)
        {
            var invitation = client.ActiveCharacter.GetInvitation(message.partyId);

            if (invitation == null)
                return;

            SendPartyInvitationDetailsMessage(client, invitation);
        }

        [WorldHandler(PartyAcceptInvitationMessage.Id)]
        public static void HandlePartyAcceptInvitationMessage(WorldClient client, PartyAcceptInvitationMessage message)
        {
            var invitation = client.ActiveCharacter.GetInvitation(message.partyId);

            if (invitation == null)
                return;

            invitation.Accept();
        }

        [WorldHandler(PartyRefuseInvitationMessage.Id)]
        public static void HandlePartyRefuseInvitationMessage(WorldClient client, PartyRefuseInvitationMessage message)
        {
            var invitation = client.ActiveCharacter.GetInvitation(message.partyId);

            if (invitation == null)
                return;

            invitation.Deny();
        }

        [WorldHandler(PartyCancelInvitationMessage.Id)]
        public static void HandlePartyCancelInvitationMessage(WorldClient client, PartyCancelInvitationMessage message)
        {
            if (!client.ActiveCharacter.IsInParty())
                return;

            var guest = client.ActiveCharacter.Party.GetGuest(message.guestId);

            if (guest == null)
                return;

            var invitation = guest.GetInvitation(client.ActiveCharacter.Party.Id);

            if (invitation == null)
                return;

            invitation.Cancel();
        }

        [WorldHandler(PartyLeaveRequestMessage.Id)]
        public static void HandlePartyLeaveRequestMessage(WorldClient client, PartyLeaveRequestMessage message)
        {
            if (!client.ActiveCharacter.IsInParty())
                return;

            // todo : check something ?

            client.ActiveCharacter.LeaveParty();
        }

        [WorldHandler(PartyAbdicateThroneMessage.Id)]
        public static void HandlePartyAbdicateThroneMessage(WorldClient client, PartyAbdicateThroneMessage message)
        {
            if (!client.ActiveCharacter.IsPartyLeader())
                return;

            var member = client.ActiveCharacter.Party.GetMember(message.playerId);

            client.ActiveCharacter.Party.ChangeLeader(member);
        }

        [WorldHandler(PartyKickRequestMessage.Id)]
        public static void HandlePartyKickRequestMessage(WorldClient client, PartyKickRequestMessage message)
        {
            if (!client.ActiveCharacter.IsPartyLeader())
                return;

            var member = client.ActiveCharacter.Party.GetMember(message.playerId);

            client.ActiveCharacter.Party.Kick(member);
        }

        public static void SendPartyKickedByMessage(WorldClient client, Character kicker)
        {
            client.Send(new PartyKickedByMessage(kicker.Id));
        }

        public static void SendPartyLeaderUpdateMessage(WorldClient client, Character leader)
        {
            client.Send(new PartyLeaderUpdateMessage(leader.Id));
        }

        public static void SendPartyRestrictedMessage(WorldClient client, bool restricted)
        {
            client.Send(new PartyRestrictedMessage(restricted));
        }

        public static void SendPartyUpdateMessage(WorldClient client, Character member)
        {
            client.Send(new PartyUpdateMessage(member.GetPartyMemberInformations()));
        }

        public static void SendPartyNewGuestMessage(WorldClient client, WorldParty party, Character guest)
        {
            client.Send(new PartyNewGuestMessage(guest.GetPartyGuestInformations(party)));
        }

        public static void SendPartyMemberRemoveMessage(WorldClient client, Character leaver)
        {
            client.Send(new PartyMemberRemoveMessage(leaver.Id));
        }

        public static void SendPartyInvitationCancelledForGuestMessage(WorldClient client, Character canceller, PartyInvitation invitation)
        {
            client.Send(new PartyInvitationCancelledForGuestMessage(invitation.Party.Id, canceller.Id));
        }

        public static void SendPartyCancelInvitationNotificationMessage(WorldClient client, PartyInvitation invitation)
        {
            client.Send(new PartyCancelInvitationNotificationMessage(
                invitation.Source.Id,
                invitation.Target.Id));
        }

        public static void SendPartyRefuseInvitationNotificationMessage(WorldClient client, PartyInvitation invitation)
        {
            client.Send(new PartyRefuseInvitationNotificationMessage(invitation.Target.Id));
        }

        public static void SendPartyDeletedMessage(WorldClient client, WorldParty party)
        {
            client.Send(new PartyDeletedMessage(party.Id));
        }

        public static void SendPartyJoinMessage(WorldClient client, WorldParty party)
        {
            client.Send(new PartyJoinMessage(party.Id,
                party.Leader.Id,
                party.Members.Select(entry => entry.GetPartyMemberInformations()),
                party.Guests.Select(entry => entry.GetPartyGuestInformations(party)),
                party.Restricted));
        }

        public static void SendPartyInvitationMessage(WorldClient client, WorldParty party, Character from)
        {
            client.Send(new PartyInvitationMessage(party.Id,
                from.Id,
                from.Name,
                client.ActiveCharacter.Id // what is that ?
                ));
        }

        public static void SendPartyInvitationDetailsMessage(WorldClient client, PartyInvitation invitation)
        {
            client.Send(new PartyInvitationDetailsMessage(
                invitation.Party.Id,
                invitation.Source.Id,
                invitation.Source.Name,
                invitation.Party.Leader.Id,
                invitation.Party.Members.Select(entry => entry.GetPartyInvitationMemberInformations())
                ));
        }    

        public static void SendPartyCannotJoinErrorMessage(WorldClient client, PartyJoinErrorEnum reason)
        {
            client.Send(new PartyCannotJoinErrorMessage((byte)reason));
        }
    }
}