﻿using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using System;

namespace Stump.Server.AuthServer.Database
{
    public class WorldCharacterRelator
    {
        public static string FetchQuery = "SELECT * FROM worlds_characters";
    }

    [TableName("worlds_characters")]
    public class WorldCharacter : IAutoGeneratedRecord
    {
        // Primitive properties

        [PrimaryKey("CharacterId", false)]
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