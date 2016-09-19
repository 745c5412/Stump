using System;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Database.Npcs.Actions;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Dialogs.Npcs;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;

namespace ArkalysPlugin.Daily
{
    internal class NpcDailyQuest
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [Variable]
        public static int NpcId = 3021;

        [Variable]
        public static int MessageContractAvailableId = 20131;

        [Variable]
        public static int MessageContractNonValidationId = 20132;

        [Variable]
        public static int MessageContractValidationId = 20133;

        [Variable]
        public static int MessageNoContractAvailableId = 20134;


        [Variable]
        public static short ReplyTakeContract = 20111;

        [Variable]
        public static short ReplyGiveContract = 20112;

        [Variable]
        public static short ReplyCancelContract = 20113;

        [Variable]
        public static short ReplyLeave = 20114;

        public static NpcMessage MessageContractAvailable;
        public static NpcMessage MessageContractNonValidation;
        public static NpcMessage MessageContractValidation;
        public static NpcMessage MessageNoContractAvailable;

        private static bool m_scriptDisabled;

        [Initialization(typeof (NpcManager), Silent = true)]
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

            MessageContractAvailable = NpcManager.Instance.GetNpcMessage(MessageContractAvailableId);
            MessageContractNonValidation = NpcManager.Instance.GetNpcMessage(MessageContractNonValidationId);
            MessageContractValidation = NpcManager.Instance.GetNpcMessage(MessageContractValidationId);
            MessageNoContractAvailable = NpcManager.Instance.GetNpcMessage(MessageNoContractAvailableId);

            if (MessageContractAvailable != null && MessageContractNonValidation != null && MessageContractValidation != null && MessageNoContractAvailable != null)
                return;

            Logger.Error("Message {0} or {1} or {2} or {3} not found, script is disabled", MessageContractAvailableId, MessageContractNonValidationId, MessageContractValidationId,
                MessageNoContractAvailableId);
            m_scriptDisabled = true;
        }

        private static void OnNpcSpawned(NpcTemplate template, Npc npc)
        {
            if (m_scriptDisabled)
                template.NpcSpawned -= OnNpcSpawned;

            npc.Actions.RemoveAll(x => x.ActionType.Contains(NpcActionTypeEnum.ACTION_TALK));
            npc.Actions.Add(new NpcDailyQuestScript());
        }
    }

    public class NpcDailyQuestScript : NpcAction
    {
        public override NpcActionTypeEnum[] ActionType
        {
            get { return new[] {NpcActionTypeEnum.ACTION_TALK}; }
        }

        public override void Execute(Npc npc, Character character)
        {
            var dialog = new NpcDailyQuestDialog(character, npc);
            dialog.Open();
        }
    }

    public class NpcDailyQuestDialog : NpcDialog
    {
        private ContractInfo m_contractInfo;

        public NpcDailyQuestDialog(Character character, Npc npc)
            : base(character, npc)
        {
        }

        public override void Open()
        {
            var contract = Character.Inventory.OfType<ContractItem>().FirstOrDefault();
            short[] replies;
            var parameters = new string[0];

            if (contract == null)
            {
                m_contractInfo = DailyQuestManager.Instance.GenerateContract(Character);

                CurrentMessage = NpcDailyQuest.MessageContractAvailable;
                replies = new[] {NpcDailyQuest.ReplyTakeContract, NpcDailyQuest.ReplyLeave};
                parameters = new[] {m_contractInfo.ToString()};
            }
            else if (contract.IsFinished() && !contract.HasBeenValidatedToday())
            {
                CurrentMessage = NpcDailyQuest.MessageContractValidation;
                replies = new[] {NpcDailyQuest.ReplyGiveContract, NpcDailyQuest.ReplyLeave};
            }
            else if (!contract.IsFinished() && !contract.HasBeenValidatedToday())
            {
                CurrentMessage = NpcDailyQuest.MessageContractNonValidation;
                replies = new[] {NpcDailyQuest.ReplyCancelContract, NpcDailyQuest.ReplyLeave};
            }
            else
            {
                CurrentMessage = NpcDailyQuest.MessageNoContractAvailable;
                replies = new[] {NpcDailyQuest.ReplyLeave};
            }

            base.Open();

            ContextRoleplayHandler.SendNpcDialogQuestionMessage(Character.Client, CurrentMessage, replies, parameters);
        }

        public override void Reply(short replyId)
        {
            if (replyId == NpcDailyQuest.ReplyTakeContract && CurrentMessage == NpcDailyQuest.MessageContractAvailable && m_contractInfo != null)
            {
                var contractItems = Character.Inventory.OfType<ContractItem>().ToArray();

                foreach (var contractItem in contractItems)
                    Character.Inventory.RemoveItem(contractItem);

                var item = Character.Inventory.AddItem(DailyQuestManager.Instance.ContractItem) as ContractItem;

                if (item == null)
                    throw new Exception("Not a contract item");

                item.InitializeEffects(m_contractInfo.Objectives);

                Close();
            }
            else if (replyId == NpcDailyQuest.ReplyGiveContract && CurrentMessage == NpcDailyQuest.MessageContractValidation)
            {
                var contractItems = Character.Inventory.OfType<ContractItem>().Where(x => !x.HasBeenValidatedToday() && x.IsFinished()).ToArray();

                foreach (var contractItem in contractItems)
                {
                    contractItem.ContractValidationDate = DateTime.Now;

                    foreach (var objective in contractItem.Objectives.Where(x => x.Item != null))
                    {
                        var item = Character.Inventory.Where(x => x.Template == objective.Item).OrderByDescending(x => x.Stack).FirstOrDefault();

                        if (item == null)
                            return;

                        if (Character.Inventory.RemoveItem(item, objective.Amount) < objective.Amount)
                            return;
                    }

                    DailyQuestManager.Instance.GiveContractRewards(Character, contractItem);
                }

                Close();
            }
            else if (replyId == NpcDailyQuest.ReplyCancelContract && CurrentMessage == NpcDailyQuest.MessageContractNonValidation)
            {
                var contractItems = Character.Inventory.OfType<ContractItem>().ToArray();

                foreach (var contractItem in contractItems)
                    Character.Inventory.RemoveItem(contractItem);

                if (m_contractInfo == null)
                    m_contractInfo = DailyQuestManager.Instance.GenerateContract(Character);
                
                ChangeMessage(NpcDailyQuest.MessageContractAvailable, new int[] {NpcDailyQuest.ReplyTakeContract, NpcDailyQuest.ReplyLeave}, m_contractInfo.ToString());
            }
            else
            {
                Close();
            }
        }
    }
}