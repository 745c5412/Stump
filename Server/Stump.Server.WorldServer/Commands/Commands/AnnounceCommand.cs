using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Misc;
using System.Drawing;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class AnnounceCommand : CommandBase
    {
        [Variable(true)]
        public static string AnnounceColor = ColorTranslator.ToHtml(Color.Red);

        public AnnounceCommand()
        {
            Aliases = new[] { "announce", "a" };
            Description = "Display an announce to all players";
            RequiredRole = RoleEnum.GameMaster_Padawan;
            AddParameter<string>("message", "msg", "The announce");
            AddParameter("target", "t", "Target", converter: ParametersConverter.CharacterConverter, isOptional: true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var color = ColorTranslator.FromHtml(AnnounceColor);

            var msg = trigger.Get<string>("msg");
            var formatMsg = trigger is GameTrigger
                                ? string.Format("(ANNOUNCE) {0} : {1}", ((GameTrigger)trigger).Character.Name, msg)
                                : string.Format("(ANNOUNCE) {0}", msg);

            if (trigger.IsArgumentDefined("target"))
            {
                formatMsg = trigger is GameTrigger
                    ? string.Format("(WARNING) {0} : {1}", ((GameTrigger)trigger).Character.Name, msg)
                    : string.Format("(WARNING) {0}", msg);

                var target = trigger.Get<Character>("target");
                target.SendServerMessage(formatMsg, color);
            }
            else
            {
                World.Instance.SendAnnounce(formatMsg, color);
            }
        }
    }

    public class AutoAnnounceCommand : CommandBase
    {
        public AutoAnnounceCommand()
        {
            Aliases = new[] { "autoannounce" };
            Description = "Add an auto announce";
            RequiredRole = RoleEnum.GameMaster;
            AddParameter<string>("message", "msg");
            AddParameter<string>("color", "c", isOptional: true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var msg = trigger.Get<string>("message");
            var color = Color.Red;

            if (trigger.IsArgumentDefined("color"))
                color = Color.FromName(trigger.Get<string>("message"));

            if (string.IsNullOrEmpty(msg))
            {
                trigger.ReplyError("You must enter an announce !");
                return;
            }

            trigger.Reply($"Announce {AutoAnnounceManager.Instance.AddAnnounce(msg, color)} added");
        }
    }

    public class AutoAnnounceRemoveCommand : CommandBase
    {
        public AutoAnnounceRemoveCommand()
        {
            Aliases = new[] { "autoannounceremove" };
            Description = "Remove an auto announce";
            RequiredRole = RoleEnum.GameMaster;
            AddParameter<int>("id", "id");
        }

        public override void Execute(TriggerBase trigger)
        {
            var id = trigger.Get<int>("id");

            if (AutoAnnounceManager.Instance.RemoveAnnounce(id))
                trigger.Reply($"Announce {id} removed");
        }
    }
}