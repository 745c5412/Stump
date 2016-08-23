using System.Drawing;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Misc;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class AnnounceCommand : CommandBase
    {
        [Variable(true)]
        public static string AnnounceColor = ColorTranslator.ToHtml(Color.Red);

        public AnnounceCommand()
        {
            Aliases = new[] {"announce", "a"};
            Description = "Display an announce to all players";
            RequiredRole = RoleEnum.GameMaster_Padawan;
            AddParameter<string>("message", "msg", "The announce");
        }

        public override void Execute(TriggerBase trigger)
        {
            var color = ColorTranslator.FromHtml(AnnounceColor);

            var msg = trigger.Get<string>("msg");
            var formatMsg = trigger is GameTrigger
                                ? string.Format("(ANNOUNCE) {0} : {1}", ((GameTrigger) trigger).Character.Name, msg)
                                : string.Format("(ANNOUNCE) {0}", msg);

            World.Instance.SendAnnounce(formatMsg, color);
        }
    }

    public class AutoAnnounceCommand : CommandBase
    {
        public AutoAnnounceCommand()
        {
            Aliases = new[] {"autoannounce"};
            Description = "Add an auto announce";
            RequiredRole = RoleEnum.GameMaster;
            AddParameter<string>("message", "msg");
        }

        public override void Execute(TriggerBase trigger)
        {
            var msg = trigger.Get<string>("msg");

            trigger.Reply($"Announce {AutoAnnounceManager.Instance.AddAnnounce(msg)} added");
        }
    }

    public class AutoAnnounceRemoveCommand : CommandBase
    {
        public AutoAnnounceRemoveCommand()
        {
            Aliases = new[] {"autoannounceremove"};
            Description = "Remove an auto announce";
            RequiredRole  = RoleEnum.GameMaster;
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