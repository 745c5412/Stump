// Generated on 11/02/2013 14:55:51
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DBSynchroniser.Records
{
    [TableName("MapPositions")]
    [D2OClass("MapPosition", "com.ankamagames.dofus.datacenter.world")]
    public class MapPositionRecord : ID2ORecord, ISaveIntercepter
    {
        private const String MODULE = "MapPositions";
        public int id;
        public int posX;
        public int posY;
        public Boolean outdoor;
        public int capabilities;

        [I18NField]
        public int nameId;

        public List<AmbientSound> sounds;
        public int subAreaId;
        public int worldMap;
        public Boolean hasPriorityOnWorldmap;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }

        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        public int PosX
        {
            get { return posX; }
            set { posX = value; }
        }

        [D2OIgnore]
        public int PosY
        {
            get { return posY; }
            set { posY = value; }
        }

        [D2OIgnore]
        public Boolean Outdoor
        {
            get { return outdoor; }
            set { outdoor = value; }
        }

        [D2OIgnore]
        public int Capabilities
        {
            get { return capabilities; }
            set { capabilities = value; }
        }

        [D2OIgnore]
        [I18NField]
        public int NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<AmbientSound> Sounds
        {
            get { return sounds; }
            set
            {
                sounds = value;
                m_soundsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_soundsBin;

        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] SoundsBin
        {
            get { return m_soundsBin; }
            set
            {
                m_soundsBin = value;
                sounds = value == null ? null : value.ToObject<List<AmbientSound>>();
            }
        }

        [D2OIgnore]
        public int SubAreaId
        {
            get { return subAreaId; }
            set { subAreaId = value; }
        }

        [D2OIgnore]
        public int WorldMap
        {
            get { return worldMap; }
            set { worldMap = value; }
        }

        [D2OIgnore]
        public Boolean HasPriorityOnWorldmap
        {
            get { return hasPriorityOnWorldmap; }
            set { hasPriorityOnWorldmap = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (MapPosition)obj;

            Id = castedObj.id;
            PosX = castedObj.posX;
            PosY = castedObj.posY;
            Outdoor = castedObj.outdoor;
            Capabilities = castedObj.capabilities;
            NameId = castedObj.nameId;
            Sounds = castedObj.sounds;
            SubAreaId = castedObj.subAreaId;
            WorldMap = castedObj.worldMap;
            HasPriorityOnWorldmap = castedObj.hasPriorityOnWorldmap;
        }

        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (MapPosition)parent : new MapPosition();
            obj.id = Id;
            obj.posX = PosX;
            obj.posY = PosY;
            obj.outdoor = Outdoor;
            obj.capabilities = Capabilities;
            obj.nameId = NameId;
            obj.sounds = Sounds;
            obj.subAreaId = SubAreaId;
            obj.worldMap = WorldMap;
            obj.hasPriorityOnWorldmap = HasPriorityOnWorldmap;
            return obj;
        }

        public virtual void BeforeSave(bool insert)
        {
            m_soundsBin = sounds == null ? null : sounds.ToBinary();
        }
    }
}