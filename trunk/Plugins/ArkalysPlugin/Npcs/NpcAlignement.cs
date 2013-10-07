﻿using NLog;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Database.Npcs.Actions;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Dialogs.Npcs;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;

namespace ArkalysPlugin.Npcs
{
    class NpcAlignement
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [Variable]
        public static int NpcId = 3002;
        //Tu veux t'engager dans la bataille? Alors choisi ton camp: Bontarien ou Brakmarien?
        [Variable]
        public static int MessageId = 20008;

        //Je vois que tu as déjà fais ton choix... Veux tu redevenir Neutre?
        [Variable]
        public static short ReplyBecomeNeutre = 20008;

        //Veux tu devenir Bontarien?
        [Variable]
        public static short ReplyBecomeAngel = 20008;

        //Veux tu devenir Brakmarien?
        [Variable]
        public static short ReplyBecomeEvil = 20008;

        public static NpcMessage Message;
        private static bool m_scriptDisabled;

        [Initialization(typeof(NpcManager), Silent = true)]
        public static void Initialize()
        {
            if (m_scriptDisabled)
                return;

            var npc = NpcManager.Instance.GetNpcTemplate(NpcId);

            if (npc == null)
            {
                Logger.Error("Npc {0} not found, script is disabled", NpcId);
                m_scriptDisabled = true;
                return;
            }

            npc.NpcSpawned += OnNpcSpawned;

            Message = NpcManager.Instance.GetNpcMessage(MessageId);

            if (Message != null)
                return;

            Logger.Error("Message {0} not found, script is disabled", MessageId);
            m_scriptDisabled = true;
        }

        private static void OnNpcSpawned(NpcTemplate template, Npc npc)
        {
            if (m_scriptDisabled)
                template.NpcSpawned -= OnNpcSpawned;

            npc.Actions.RemoveAll(x => x.ActionType == NpcActionTypeEnum.ACTION_TALK);
            npc.Actions.Add(new NpcAlignementScript());
        }
    }

    public class NpcAlignementScript : NpcAction
    {
        public override NpcActionTypeEnum ActionType
        {
            get { return NpcActionTypeEnum.ACTION_TALK; }
        }

        public override void Execute(Npc npc, Character character)
        {
            var dialog = new NpcAlignementDialog(character, npc);
            dialog.Open();
        }
    }

    public class NpcAlignementDialog : NpcDialog
    {
        public NpcAlignementDialog(Character character, Npc npc) : base(character, npc)
        {
            CurrentMessage = NpcAlignement.Message;
        }

        public override void Open()
        {
            base.Open();

            ContextRoleplayHandler.SendNpcDialogQuestionMessage(Character.Client, CurrentMessage,
                                                                Character.AlignmentSide !=
                                                                AlignmentSideEnum.ALIGNMENT_NEUTRAL
                                                                    ? new[] {NpcAlignement.ReplyBecomeNeutre}
                                                                    : new[]
                                                                        {
                                                                            NpcAlignement.ReplyBecomeAngel,
                                                                            NpcAlignement.ReplyBecomeAngel
                                                                        });
        }

        public override void Reply(short replyId)
        {
            if (replyId == NpcAlignement.ReplyBecomeAngel)
            {
                Character.ChangeAlignementSide(AlignmentSideEnum.ALIGNMENT_ANGEL);
                
            }
            else if (replyId == NpcAlignement.ReplyBecomeEvil)
            {
                Character.ChangeAlignementSide(AlignmentSideEnum.ALIGNMENT_EVIL);
            }
            else if (replyId == NpcAlignement.ReplyBecomeNeutre)
            {
                Character.ChangeAlignementSide(AlignmentSideEnum.ALIGNMENT_NEUTRAL);
            }

            Close();
        }
    }
}
