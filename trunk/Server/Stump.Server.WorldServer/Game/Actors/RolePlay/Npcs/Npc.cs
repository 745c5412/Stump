using System.Linq;
using Stump.Core.Cache;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs
{
    public sealed class Npc : RolePlayActor
    {
        public Npc(int id, NpcTemplate template, ObjectPosition position, EntityLook look)
        {
            Id = id;
            Template = template;
            Position = position;
            Look = look;

            m_gameContextActorInformations = new ObjectValidator<GameContextActorInformations>(BuildGameContextActorInformations);
        }

        public Npc(int id, NpcSpawn spawn)
            : this (id, spawn.Template, spawn.GetPosition(), spawn.Look)
        {
            Spawn = spawn;
        }

        public NpcTemplate Template
        {
            get;
            private set;
        }

        public NpcSpawn Spawn
        {
            get;
            private set;
        }

        public override int Id
        {
            get;
            protected set;
        }

        public int TemplateId
        {
            get { return Template.Id; }
        }

        public override EntityLook Look
        {
            get;
            set;
        }

        public void Refresh()
        {
            m_gameContextActorInformations.Invalidate();

            if (Map != null)
                Map.Refresh(this);
        }

        public void InteractWith(NpcActionTypeEnum actionType, Character dialoguer)
        {
            if (!CanInteractWith(actionType, dialoguer))
                return;

            var actions = Template.GetNpcActions(actionType);

            actions.First(entry => entry.ConditionaExpression == null || entry.ConditionaExpression.Eval(dialoguer))
                .Execute(this, dialoguer);
        }

        public bool CanInteractWith(NpcActionTypeEnum action, Character dialoguer)
        {
            if (dialoguer.Map != Position.Map)
                return false;

            var actions = Template.GetNpcActions(action);

            return actions.Length > 0 && actions.Any(entry => entry.ConditionaExpression == null || 
                entry.ConditionaExpression.Eval(dialoguer));
        }

        public void SpeakWith(Character dialoguer)
        {
            if (!CanInteractWith(NpcActionTypeEnum.ACTION_TALK, dialoguer))
                return;

            InteractWith(NpcActionTypeEnum.ACTION_TALK, dialoguer);
        }

        #region GameContextActorInformations

        private readonly ObjectValidator<GameContextActorInformations> m_gameContextActorInformations;

        private GameContextActorInformations BuildGameContextActorInformations()
        {
            return new GameRolePlayNpcInformations(Id,
                                                   Look,
                                                   GetEntityDispositionInformations(),
                                                   (short)Template.Id,
                                                   Template.Gender != 0,
                                                   Template.SpecialArtworkId);
        }

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            return m_gameContextActorInformations;
        }

        #endregion

        public override string ToString()
        {
            return string.Format("{0} ({1}) [{2}]", Template.Name, Id, TemplateId);
        }
    }
}