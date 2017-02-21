 


// Generated on 02/19/2017 13:43:57
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("FinishMoves")]
    [D2OClass("FinishMove", "com.ankamagames.dofus.datacenter.spells")]
    public class FinishMoveRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "FinishMoves";
        public int id;
        public int duration;
        public Boolean free;
        [I18NField]
        public uint nameId;
        public int category;
        public int spellLevel;

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
        public int Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        [D2OIgnore]
        public Boolean Free
        {
            get { return free; }
            set { free = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [D2OIgnore]
        public int Category
        {
            get { return category; }
            set { category = value; }
        }

        [D2OIgnore]
        public int SpellLevel
        {
            get { return spellLevel; }
            set { spellLevel = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (FinishMove)obj;
            
            Id = castedObj.id;
            Duration = castedObj.duration;
            Free = castedObj.free;
            NameId = castedObj.nameId;
            Category = castedObj.category;
            SpellLevel = castedObj.spellLevel;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (FinishMove)parent : new FinishMove();
            obj.id = Id;
            obj.duration = Duration;
            obj.free = Free;
            obj.nameId = NameId;
            obj.category = Category;
            obj.spellLevel = SpellLevel;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}