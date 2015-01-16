﻿using System;
using System.Linq;
using Stump.Core.Extensions;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Mounts;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Handlers.Inventory;
using Stump.Server.WorldServer.Handlers.Mounts;
using MapPaddock = Stump.Server.WorldServer.Game.Maps.Paddocks.Paddock;
using Mount = Stump.Server.WorldServer.Game.Actors.RolePlay.Mounts.Mount;

namespace Stump.Server.WorldServer.Game.Exchanges.Paddock
{
    public class PaddockExchanger : Exchanger
    {
        public PaddockExchanger(Character character, MapPaddock paddock, PaddockExchange paddockExchange)
            : base(paddockExchange)
        {
            Character = character;
            Paddock = paddock;
        }

        public Character Character
        {
            get;
            private set;
        }

        public MapPaddock Paddock
        {
            get;
            private set;
        }

        public void StoreMount(Mount mount)
        {
            var item = ItemManager.Instance.CreatePlayerItem(Character, 7806, 1);

            var date = DateTime.Now;

            var nameEffect = new EffectString((short)EffectsEnum.Effect_Name, mount.Name, new EffectBase());
            var belongEffect = new EffectString((short)EffectsEnum.Effect_BelongsTo, Character.Name, new EffectBase());
            var validityEffect = new EffectDuration((short)EffectsEnum.Effect_Validity, 39, 23, 59, new EffectBase());
            var mountEffect = new EffectMount((short)EffectsEnum.Effect_ViewMountCharacteristics, mount.Id, date.GetUnixTimeStampDouble(), mount.ModelId, new EffectBase());

            item.Effects.Add(nameEffect);
            item.Effects.Add(belongEffect);
            item.Effects.Add(mountEffect);
            item.Effects.Add(validityEffect);

            Character.Inventory.AddItem(item);
        }

        public void EquipMount(Mount mount)
        {
            mount.Owner = Character;
            Character.Mount = mount;

            MountManager.Instance.LinkMountToCharacter(Character, mount);
            MountHandler.SendMountSetMessage(Character.Client, mount.GetMountClientData());
        }

        public bool HasMountRight(Mount mount, bool equip = false)
        {
            if (equip && Character.HasEquipedMount())
                return false;

            if (mount.Owner != null && Character.Id != mount.OwnerId)
                return false;

            if (!equip || Character.Level >= Mount.RequiredLevel)
                return true;

            Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 227, Mount.RequiredLevel);
            return false;
        }

        public Mount GetMountByItem(BasePlayerItem item)
        {
            var effect = item.Effects.FirstOrDefault(x => x.GetEffectInstance() is EffectInstanceMount);
            if (effect == null)
                return null;

            var effectInstance = effect.GetEffectInstance() as EffectInstanceMount;
            return MountManager.Instance.GetMountById((int)effectInstance.mountId);
        }

        public bool EquipToPaddock(int mountId)
        {
            if (!HasMountRight(Character.Mount))
                return false;

            if (Character.Mount.Id != mountId)
                return false;

            Paddock.AddMountToPaddock(Character.Mount);
            InventoryHandler.SendExchangeMountPaddockAddMessage(Character.Client, Character.Mount);

            Character.Mount.Release(Character);

            return true;
        }

        public bool EquipToStable(int mountId)
        {
            if (!HasMountRight(Character.Mount))
                return false;

            if (Character.Mount.Id != mountId)
                return false;

            Paddock.AddMountToStable(Character.Mount);
            InventoryHandler.SendExchangeMountStableAddMessage(Character.Client, Character.Mount);

            Character.Mount.Release(Character);

            return true;
        }

        public bool PaddockToEquip(int mountId)
        {
            var mount = Paddock.GetPaddockedMount(mountId);
            if (mount == null)
                return false;

            if (!HasMountRight(mount, true))
                return false;

            Paddock.RemoveMountFromPaddock(mount);
            InventoryHandler.SendExchangeMountPaddockRemoveMessage(Character.Client, mount);

            EquipMount(mount);

            return true;
        }

        public bool PaddockToStable(int mountId)
        {
            var mount = Paddock.GetPaddockedMount(mountId);
            if (mount == null)
                return false;

            if (!HasMountRight(mount))
                return false;

            Paddock.RemoveMountFromPaddock(mount);
            Paddock.AddMountToStable(mount);

            InventoryHandler.SendExchangeMountPaddockRemoveMessage(Character.Client, mount);
            InventoryHandler.SendExchangeMountStableAddMessage(Character.Client, mount);

            return true;
        }

        public bool StableToPaddock(int mountId)
        {
            var mount = Paddock.GetStabledMount(mountId);
            if (mount == null)
                return false;

            if (!HasMountRight(mount))
                return false;

            Paddock.RemoveMountFromStable(mount);
            Paddock.AddMountToPaddock(mount);

            InventoryHandler.SendExchangeMountStableRemoveMessage(Character.Client, mount);
            InventoryHandler.SendExchangeMountPaddockAddMessage(Character.Client, mount);

            return true;
        }

        public bool StableToEquip(int mountId)
        {
            var mount = Paddock.GetStabledMount(mountId);
            if (mount == null)
                return false;

            if (!HasMountRight(mount, true))
                return false;

            Paddock.RemoveMountFromStable(mount);
            InventoryHandler.SendExchangeMountStableRemoveMessage(Character.Client, mount);

            EquipMount(mount);

            return true;
        }

        public bool StableToInventory(int mountId)
        {
            var mount = Paddock.GetStabledMount(mountId);
            if (mount == null)
                return false;

            if (!HasMountRight(mount))
                return false;

            Paddock.RemoveMountFromStable(mount);
            InventoryHandler.SendExchangeMountStableRemoveMessage(Character.Client, mount);

            StoreMount(mount);

            return true;
        }

        public bool PaddockToInventory(int mountId)
        {
            var mount = Paddock.GetPaddockedMount(mountId);
            if (mount == null)
                return false;

            if (!HasMountRight(mount))
                return false;

            Paddock.RemoveMountFromPaddock(mount);
            InventoryHandler.SendExchangeMountPaddockRemoveMessage(Character.Client, mount);

            StoreMount(mount);

            return true;
        }

        public bool EquipToInventory(int mountId)
        {
            if (!HasMountRight(Character.Mount))
                return false;

            if (Character.Mount.Id != mountId)
                return false;

            StoreMount(Character.Mount);

            Character.Mount.Release(Character);

            return true;
        }

        public bool InventoryToStable(int itemId)
        {
            var item = Character.Inventory.TryGetItem(itemId);
            var mount = GetMountByItem(item);
            if (mount == null)
                return false;

            mount.Owner = Character;
            Paddock.AddMountToStable(mount);
            Character.Inventory.RemoveItem(item);

            InventoryHandler.SendExchangeMountStableAddMessage(Character.Client, mount);

            return true;
        }

        public bool InventoryToPaddock(int itemId)
        {
            var item = Character.Inventory.TryGetItem(itemId);
            var mount = GetMountByItem(item);
            if (mount == null)
                return false;

            mount.Owner = Character;
            Paddock.AddMountToPaddock(mount);
            Character.Inventory.RemoveItem(item);

            InventoryHandler.SendExchangeMountPaddockAddMessage(Character.Client, mount);

            return true;
        }

        public bool InventoryToEquip(int itemId)
        {
            if (Character.HasEquipedMount())
                return false;

            var item = Character.Inventory.TryGetItem(itemId);
            var mount = GetMountByItem(item);
            if (mount == null)
                return false;

            mount.Owner = Character;
            Character.Inventory.RemoveItem(item);

            EquipMount(mount);

            return true;
        }
        public override bool MoveItem(int id, int quantity)
        {
            return false;
        }

        public override bool SetKamas(int amount)
        {
            return false;
        }
    }
}
