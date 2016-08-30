﻿using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Maps.Cells;
using System;
using System.Collections.Generic;

namespace Stump.Server.WorldServer.Game.Maps
{
    public sealed class WorldObjectItem : WorldObject
    {
        public WorldObjectItem(int id, Map map, Cell cell, ItemTemplate template, List<EffectBase> effects, int quantity)
        {
            Id = id;
            Position = new ObjectPosition(map, cell);
            Quantity = quantity;
            Item = template;
            Effects = effects;
            SpawnDate = DateTime.Now;
        }

        public override int Id
        {
            get;
            protected set;
        }

        public ItemTemplate Item
        {
            get;
            protected set;
        }

        public List<EffectBase> Effects
        {
            get;
            protected set;
        }

        public int Quantity
        {
            get;
            protected set;
        }

        public DateTime SpawnDate
        {
            get;
            protected set;
        }
    }
}