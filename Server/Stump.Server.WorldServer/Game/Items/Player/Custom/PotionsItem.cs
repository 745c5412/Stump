﻿using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs.Guilds;
using Stump.Server.WorldServer.Handlers.Mounts;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemId(ItemIdEnum.BontarianIntercityExpressPotion)]
    public class BontarianPotion : BasePlayerItem
    {
        [Variable]
        private const int m_destinationMap = 5506048;

        [Variable]
        private const int m_destinationCell = 359;

        public BontarianPotion(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            var map = World.Instance.GetMap(m_destinationMap);
            var cell = map.Cells[m_destinationCell];

            Owner.Teleport(map, cell);

            return 1;
        }
    }

    [ItemId(ItemIdEnum.BrakmarianIntercityExpressPotion)]
    public class BrakmarianPotion : BasePlayerItem
    {
        [Variable]
        private const int m_destinationMap = 13631488;

        [Variable]
        private const int m_destinationCell = 373;

        public BrakmarianPotion(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            var map = World.Instance.GetMap(m_destinationMap);
            var cell = map.Cells[m_destinationCell];

            Owner.Teleport(map, cell);

            return 1;
        }
    }

    [ItemId(ItemIdEnum.NamechangePotion)]
    public class NameChangePotion : BasePlayerItem
    {
        public NameChangePotion(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            if (Owner.Record.Rename || Owner.Record.Recolor || Owner.Record.Relook > 0)
            {
                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_POPUP, 43);
                return 0;
            }

            Owner.Record.Rename = true;
            Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_POPUP, 41);

            return 1;
        }
    }

    [ItemId(ItemIdEnum.ColourchangePotion)]
    public class ColourChangePotion : BasePlayerItem
    {
        public ColourChangePotion(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            if (Owner.Record.Rename || Owner.Record.Recolor || Owner.Record.Relook > 0)
            {
                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_POPUP, 43);
                return 0;
            }

            Owner.Record.Recolor = true;
            Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_POPUP, 42);

            return 1;
        }
    }

    [ItemId(ItemIdEnum.LookChangePotion)]
    public class LookChangePotion : BasePlayerItem
    {
        public LookChangePotion(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            if (Owner.Record.Rename || Owner.Record.Recolor || Owner.Record.Relook > 0)
            {
                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_POPUP, 43);
                return 0;
            }

            Owner.Record.Relook = 1;
            Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_POPUP, 58);

            return 1;
        }
    }

    [ItemId(ItemIdEnum.SexchangePotion)]
    public class SexChangePotion : BasePlayerItem
    {
        public SexChangePotion(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            if (Owner.Record.Rename || Owner.Record.Recolor || Owner.Record.Relook > 0)
            {
                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_POPUP, 43);
                return 0;
            }

            Owner.Record.Relook = 2;
            Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_POPUP, 44);

            return 1;
        }
    }

    [ItemId(ItemIdEnum.GuildNameChangePotion)]
    public class GuildNameChangePotion : BasePlayerItem
    {
        public GuildNameChangePotion(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            if (Owner.GuildMember == null)
                return 0;

            if (!Owner.GuildMember.IsBoss)
                return 0;

            var panel = new GuildModificationPanel(Owner) { ChangeName = true, ChangeEmblem = false };
            panel.Open();

            return 0;
        }
    }

    [ItemId(ItemIdEnum.GuildEmblemChangePotion)]
    public class GuildEmblemChangePotion : BasePlayerItem
    {
        public GuildEmblemChangePotion(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            if (Owner.GuildMember == null)
                return 0;

            if (!Owner.GuildMember.IsBoss)
                return 0;

            var panel = new GuildModificationPanel(Owner) { ChangeName = false, ChangeEmblem = true };
            panel.Open();

            return 0;
        }
    }

    [ItemId(20838)]
    public class ChameleonBehaviorPotion : BasePlayerItem
    {
        public ChameleonBehaviorPotion(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            if (!Owner.HasEquippedMount())
                return 0;

            if (Owner.EquippedMount.Behaviors.Contains((int)MountBehaviorEnum.Caméléone))
                return 0;

            Owner.EquippedMount.AddBehavior(MountBehaviorEnum.Caméléone);

            MountHandler.SendMountSetMessage(Owner.Client, Owner.EquippedMount.GetMountClientData());

            return 1;
        }
    }
}
