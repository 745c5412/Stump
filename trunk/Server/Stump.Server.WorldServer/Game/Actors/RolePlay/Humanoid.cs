using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Guilds;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay
{
    public abstract class Humanoid : NamedActor
    {
        private List<RolePlayActor> m_followingCharacters = new List<RolePlayActor>();

        public IEnumerable<RolePlayActor> FollowingCharacters
        {
            get { return m_followingCharacters; }
        }

        public void AddFollowingCharacter(RolePlayActor actor)
        {
            m_followingCharacters.Add(actor);
        }

        public void RemoveFollowingCharacter(RolePlayActor actor)
        {
            m_followingCharacters.Remove(actor);
        }

        public virtual SexTypeEnum Sex
        {
            get;
            protected set;
        }

        #region Network

        #region HumanInformations

        public virtual HumanInformations GetHumanInformations()
        {
            return new HumanInformations(new ActorRestrictionsInformations(),
                Sex == SexTypeEnum.SEX_FEMALE,
                Enumerable.Empty<HumanOption>()); // todo
        }

        #endregion 

	    #endregion
    }
}