using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;

namespace Stump.Server.WorldServer.Database.Npcs.Replies
{
    [Discriminator("LearnSpell", typeof(NpcReply), typeof(NpcReplyRecord))]
    public class LearnSpellReply : NpcReply
    {
        public LearnSpellReply(NpcReplyRecord record)
            : base(record)
        {
        }

        /// <summary>
        /// Parameter 0
        /// </summary>
        public int SpellId
        {
            get
            {
                return Record.GetParameter<int>(0);
            }
            set
            {
                Record.SetParameter(0, value);
            }
        }

        public override bool Execute(Npc npc, Character character)
        {
            if (!base.Execute(npc, character))
                return false;

            if (!character.Spells.HasSpell(SpellId))
                character.Spells.LearnSpell(SpellId);

            return true;
        }
    }
}