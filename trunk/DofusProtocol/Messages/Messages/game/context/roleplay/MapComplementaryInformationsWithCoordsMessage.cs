
// Generated on 01/04/2013 14:35:47
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MapComplementaryInformationsWithCoordsMessage : MapComplementaryInformationsDataMessage
    {
        public const uint Id = 6268;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short worldX;
        public short worldY;
        
        public MapComplementaryInformationsWithCoordsMessage()
        {
        }
        
        public MapComplementaryInformationsWithCoordsMessage(short subAreaId, int mapId, sbyte subareaAlignmentSide, IEnumerable<Types.HouseInformations> houses, IEnumerable<Types.GameRolePlayActorInformations> actors, IEnumerable<Types.InteractiveElement> interactiveElements, IEnumerable<Types.StatedElement> statedElements, IEnumerable<Types.MapObstacle> obstacles, IEnumerable<Types.FightCommonInformations> fights, short worldX, short worldY)
         : base(subAreaId, mapId, subareaAlignmentSide, houses, actors, interactiveElements, statedElements, obstacles, fights)
        {
            this.worldX = worldX;
            this.worldY = worldY;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            worldX = reader.ReadShort();
            if (worldX < -255 || worldX > 255)
                throw new Exception("Forbidden value on worldX = " + worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
            worldY = reader.ReadShort();
            if (worldY < -255 || worldY > 255)
                throw new Exception("Forbidden value on worldY = " + worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
        }
        
    }
    
}