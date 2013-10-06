 


// Generated on 10/06/2013 14:22:00
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("Url")]
    [D2OClass("Url")]
    public class UrlRecord : ID2ORecord
    {
        private const String MODULE = "Url";
        public int id;
        public int browserId;
        public String url;
        public String param;
        public String method;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int BrowserId
        {
            get { return browserId; }
            set { browserId = value; }
        }

        [NullString]
        public String Url
        {
            get { return url; }
            set { url = value; }
        }

        [NullString]
        public String Param
        {
            get { return param; }
            set { param = value; }
        }

        [NullString]
        public String Method
        {
            get { return method; }
            set { method = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Url)obj;
            
            Id = castedObj.id;
            BrowserId = castedObj.browserId;
            Url = castedObj.url;
            Param = castedObj.param;
            Method = castedObj.method;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new Url();
            obj.id = Id;
            obj.browserId = BrowserId;
            obj.url = Url;
            obj.param = Param;
            obj.method = Method;
            return obj;
        
        }
    }
}