using Stump.Server.BaseServer.Plugins;
using System;

namespace MessageLogger
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
            get { return "MessageLogger"; }
        }

        public override string Description
        {
            get { return "Log Messages"; }
        }

        public override string Author
        {
            get { return "AZOTE"; }
        }

        public override bool AllowConfigUpdate
        {
            get { return true; }
        }

        public override bool UseConfig
        {
            get { return true; }
        }

        public override string ConfigFileName
        {
            get { return "message_logger.xml"; }
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