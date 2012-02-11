using System.Collections.Generic;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.Game.Maps.Cells.Shapes
{
    public class Line : IShape
    {
        public Line(byte radius)
        {
            Radius = radius;
            Direction = DirectionsEnum.DOWN_RIGHT;
        }

        #region IShape Members

        public uint Surface
        {
            get
            {
                return (uint)Radius + 1;
            }
        }

        public byte MinRadius
        {
            get;
            set;
        }

        public DirectionsEnum Direction
        {
            get;
            set;
        }

        public byte Radius
        {
            get;
            set;
        }

        public Cell[] GetCells(Cell centerCell, Map map)
        {
            var centerPoint = new MapPoint(centerCell);
            var result = new List<Cell>();

            for (int i = (int) MinRadius; i <= Radius; i++)
            {
                switch (Direction)
                {
                    case DirectionsEnum.LEFT:
                        if (MapPoint.IsInMap(centerPoint.X - i, centerPoint.Y - i))
                            AddCellIfValid(centerPoint.X - i, centerPoint.Y - i, map, result);
                        break;
                    case DirectionsEnum.UP:
                        if (MapPoint.IsInMap(centerPoint.X - i, centerPoint.Y + i))
                            AddCellIfValid(centerPoint.X - i, centerPoint.Y - i, map, result);
                        break;
                    case DirectionsEnum.RIGHT:
                        if (MapPoint.IsInMap(centerPoint.X + i, centerPoint.Y + i))
                            AddCellIfValid(centerPoint.X - i, centerPoint.Y - i, map, result);
                        break;
                    case DirectionsEnum.DOWN:
                        if (MapPoint.IsInMap(centerPoint.X + i, centerPoint.Y - i))
                            AddCellIfValid(centerPoint.X - i, centerPoint.Y - i, map, result);
                        break;
                    case DirectionsEnum.UP_LEFT:
                        if (MapPoint.IsInMap(centerPoint.X - i, centerPoint.Y))
                            AddCellIfValid(centerPoint.X - i, centerPoint.Y - i, map, result);
                        break;
                    case DirectionsEnum.DOWN_LEFT:
                        if (MapPoint.IsInMap(centerPoint.X, centerPoint.Y - i))
                            AddCellIfValid(centerPoint.X - i, centerPoint.Y - i, map, result);
                        break;
                    case DirectionsEnum.DOWN_RIGHT:
                        if (MapPoint.IsInMap(centerPoint.X + i, centerPoint.Y))
                            AddCellIfValid(centerPoint.X - i, centerPoint.Y - i, map, result);
                        break;
                    case DirectionsEnum.UP_RIGHT:
                        if (MapPoint.IsInMap(centerPoint.X, centerPoint.Y + i))
                            AddCellIfValid(centerPoint.X - i, centerPoint.Y - i, map, result);
                        break;
                }
            }

            return result.ToArray();
        }

        private static void AddCellIfValid(int x, int y, Map map, IList<Cell> container)
        {
            if (!MapPoint.IsInMap(x, y))
                return;

            container.Add(map.Cells[MapPoint.CoordToCellId(x, y)]);
        }
        #endregion
    }
}