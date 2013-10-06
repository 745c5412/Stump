 


// Generated on 10/06/2013 14:21:58
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("EmblemBackgrounds")]
    [D2OClass("EmblemBackground")]
    public class EmblemBackgroundRecord : ID2ORecord
    {
        private const String MODULE = "EmblemBackgrounds";
        public int id;
        public int order;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (EmblemBackground)obj;
            
            Id = castedObj.id;
            Order = castedObj.order;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new EmblemBackground();
            obj.id = Id;
            obj.order = Order;
            return obj;
        
        }
    }
}