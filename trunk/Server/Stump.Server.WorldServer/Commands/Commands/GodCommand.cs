using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;

namespace Stump.Server.WorldServer.Commands.Commands
{

    public class GodCommand : SubCommandContainer
    {
        public GodCommand()
        {
            Aliases = new[] { "god" };
            RequiredRole = RoleEnum.GameMaster;
            Description = "Just to be all powerfull.";
        }
    }


    public class GodOnCommand : TargetSubCommand
    {
        public GodOnCommand()
        {
            Aliases = new[] { "on" };
            RequiredRole = RoleEnum.GameMaster;
            ParentCommand = typeof(GodCommand);
            Description = "Activate god mode";
            AddTargetParameter(true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var target = GetTarget(trigger);

            target.ToggleGodMode(true);
            trigger.Reply("You are god !");
        }
    }
    public class GodOffCommand : TargetSubCommand
    {
        public GodOffCommand()
        {
            Aliases = new[] { "off" };
            RequiredRole = RoleEnum.Administrator;
            ParentCommand = typeof(GodCommand);
            Description = "Disable god mode";
            AddTargetParameter(true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var target = GetTarget(trigger);

            target.ToggleGodMode(false);
            trigger.Reply("You'r not god more");
        }
    }

    public class LevelUpCommand : TargetCommand
    {
        public LevelUpCommand()
        {
            Aliases = new[] { "levelup" };
            RequiredRole = RoleEnum.GameMaster;
            Description = "Gives some levels to the target";
            AddParameter("amount", "amount", "Amount of levels to add", (short)1);
            AddTargetParameter(true, "Character who will level up");
        }

        public override void Execute(TriggerBase trigger)
        {
            var target = GetTarget(trigger);
            byte delta;

            var amount = trigger.Get<short>("amount");
            if (amount > 0 && amount <= byte.MaxValue)
            {
                delta = (byte) (amount);
                target.LevelUp(delta);
                trigger.Reply("Added " + trigger.Bold("{0}") + " levels to '{1}'.", delta, target.Name);

            }
            else if (amount < 0 && -amount <= byte.MaxValue)
            {
                delta = (byte)( -amount );
                target.LevelDown(delta);
                trigger.Reply("Removed " + trigger.Bold("{0}") + " levels from '{1}'.", delta, target.Name);

            }
            else
            {
                trigger.ReplyError("Invalid level given. Must be greater then -255 and lesser than 255");
            }
        }
    }

    public class SetKamasCommand : TargetCommand
    {
        public SetKamasCommand()
        {
            Aliases = new[] { "kamas" };
            RequiredRole = RoleEnum.GameMaster;
            Description = "Set the amount kamas of target's inventory";
            AddParameter<int>("amount", "amount", "Amount of kamas to set");
            AddTargetParameter(true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var target = GetTarget(trigger);
            var kamas = trigger.Get<int>("amount");

            target.Inventory.SetKamas(kamas);
            trigger.ReplyBold("{0} has now {1} kamas", target, kamas);
        }
    }

    public class SetStatsCommand : TargetCommand
    {
        public SetStatsCommand()
        {
            Aliases = new[] { "stats" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Set the amount of stats point of the target";
            AddParameter<ushort>("amount", "amount", "Amount of stats points to set");
            AddTargetParameter(true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var target = GetTarget(trigger);
            var statsPoints = trigger.Get<ushort>("amount");

            target.StatsPoints = statsPoints;
            target.RefreshStats();
            trigger.Reply("{0} has now {1} stats points", target, statsPoints);
        }
    }

    public class InvisibleCommand : TargetCommand
    {
        public InvisibleCommand()
        {
            Aliases = new[] { "invisible", "setinv" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Toggle invisible state";
            AddTargetParameter(true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var target = GetTarget(trigger);

            trigger.Reply(target.ToggleInvisibility() ? "{0} is now invisible" : "{0} is now visible", target);
        }
    }

    public class TranquilityCommand : CommandBase
    {
        public TranquilityCommand()
        {
            Aliases = new[] { "tranquility" };
            RequiredRole = RoleEnum.GameMaster;
            Description = "Toggle tranquility mod";
        }

        public override void Execute(TriggerBase trigger)
        {
            var player = ((GameTrigger) trigger).Character;

            trigger.Reply(player.ToggleAway() ? "Tranquility mode is now ON" : "Tranquility mode is now OFF");
        }
    }
}