using Stump.Server.WorldServer.Game.Maps.Pathfinding;
using System;

namespace Stump.Server.WorldServer.Game.Actors.Interfaces
{
    public interface IAutoMovedEntity
    {
        DateTime NextMoveDate
        {
            get;
            set;
        }

        DateTime LastMoveDate
        {
            get;
        }

        bool StartMove(Path path);
    }
}