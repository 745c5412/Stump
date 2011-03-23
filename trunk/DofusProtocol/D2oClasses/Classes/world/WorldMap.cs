using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.world
{
	
	[D2OClass("WorldMaps")]
	public class WorldMap : Object
	{
		internal const String MODULE = "WorldMaps";
		public int id;
		public int origineX;
		public int origineY;
		public double mapWidth;
		public double mapHeight;
		public uint horizontalChunck;
		public uint verticalChunck;
		public Boolean viewableEverywhere;
		public double minScale;
		public double maxScale;
		public double startScale;
		public int centerX;
		public int centerY;
		public int totalWidth;
		public int totalHeight;
		
	}
}
