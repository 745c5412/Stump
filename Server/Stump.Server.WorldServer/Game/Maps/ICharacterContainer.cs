using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System;
using System.Collections.Generic;

namespace Stump.Server.WorldServer.Game.Maps
{
    public interface ICharacterContainer
    {
        IEnumerable<Character> GetAllCharacters();

        void ForEach(Action<Character> action);

        WorldClientCollection Clients
        {
            get;
        }
    }
}