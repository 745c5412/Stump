﻿using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Accounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Social
{
    public class Friend
    {
        public Friend(AccountRelation relation, WorldAccount account)
        {
            Relation = relation;
            Account = account;
        }

        public Friend(AccountRelation relation, WorldAccount account, Character character)
        {
            Relation = relation;
            Account = account;
            Character = character;
        }

        public WorldAccount Account
        {
            get;
            private set;
        }

        public Character Character
        {
            get;
            private set;
        }

        public AccountRelation Relation
        {
            get;
            private set;
        }

        public void SetOnline(Character character)
        {
            if (character.Client.WorldAccount.Id != Account.Id)
                return;

            Character = character;
        }

        public void SetOffline()
        {
            Character = null;
        }

        public bool IsOnline()
        {
            return Character != null;
        }

        public FriendInformations GetFriendInformations()
        {
            if (IsOnline())
            {
                return new FriendOnlineInformations(Account.Id,
                    Account.Nickname,
                    (sbyte)(Character.IsFighting() ? PlayerStateEnum.GAME_TYPE_FIGHT : PlayerStateEnum.GAME_TYPE_ROLEPLAY),
                    Account.LastConnectionTimeStamp,
                    0, // todo achievement
                    Character.Name,
                    Character.Level,
                    (sbyte)Character.AlignmentSide,
                    (sbyte)Character.Breed.Id,
                    Character.Sex == SexTypeEnum.SEX_FEMALE,
                    Character.GuildMember == null ? new BasicGuildInformations(0, "") : Character.GuildMember.Guild.GetBasicGuildInformations(),
                    -1);
            }

            return new FriendInformations(
                Account.Id,
                Account.Nickname,
                (sbyte)PlayerStateEnum.NOT_CONNECTED,
                Account.LastConnectionTimeStamp,
                0); // todo achievement
        }
    }
}