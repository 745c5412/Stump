﻿using Dapper.Contrib.Extensions;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace Stump.Server.AuthServer.Database
{
    public class WorldCharacterRelator
    {
        public static string FetchQuery = "SELECT * FROM worlds_characters";
    }

    [TableName("worlds_characters")]
    [Table("worlds_characters")]
    public class WorldCharacter : DbObject<WorldCharacter>, IAutoGeneratedRecord
    {
        // Primitive properties

        public long Id
        {
            get;
            set;
        }

        [PrimaryKey("CharacterId", false)]
        [Key]
        [Write(true)]
        public int CharacterId
        {
            get;
            set;
        }
        public int AccountId
        {
            get;
            set;
        }
        public int WorldId
        {
            get;
            set;
        }
    }
}