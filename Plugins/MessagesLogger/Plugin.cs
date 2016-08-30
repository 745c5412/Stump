using Stump.Server.BaseServer.Plugins;
using System;

namespace MessagesLogger
{
    public class Plugin : PluginBase
    {
        public Plugin(PluginContext context)
            : base(context)
        {
            CurrentPlugin = this;
        }

        public override string Name
        {
            get { return "MessagesLogger"; }
        }

        public override string Description
        {
            get { return "Log Messages"; }
        }

        public override string Author
        {
            get { return "Orochi"; }
        }

        public override bool AllowConfigUpdate
        {
            get { return false; }
        }

        public override bool UseConfig
        {
            get { return false; }
        }

        public override Version Version
        {
            get { return new Version(1, 0); }
        }

        public override void Initialize()
        {
            base.Initialize();
            Initialized = true;
        }

        public override void Shutdown()
        {
            base.Shutdown();
            Initialized = false;
        }

        public override void Dispose()
        {
        }

        public static Plugin CurrentPlugin
        {
            get;
            private set;
        }

        public bool Initialized
        {
            get;
            private set;
        }
    }
}