﻿using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Usables
{
    public abstract class UsableEffectHandler : EffectHandler
    {
        protected UsableEffectHandler(EffectBase effect, Character target, PlayerItem item)
            : base (effect)
        {
            Target = target;
            Item = item;
            NumberOfUses = 1;
        }

        public Character Target
        {
            get;
            protected set;
        }

        public PlayerItem Item
        {
            get;
            protected set;
        }

        public Cell TargetCell
        {
            get;
            set;
        }

        public uint NumberOfUses
        {
            get;
            set;
        }

        public uint UsedItems
        {
            get;
            protected set;
        }
    }
}