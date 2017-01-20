using Stump.Server.BaseServer.Plugins;
using System;

namespace GameplayPlugin
{
    public class Plugin : PluginBase
    {
        public Plugin(PluginContext context)
            : base(context)
        {
            CurrentPlugin = this;
        }

        public override string Name => "Gameplay Plugin";

        public override string Description => "This plugin manage the gameplay";

        public override string Author => "Azote";

        public override Version Version => new Version(1, 0);

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

        public override bool UseConfig => true;

        public override string ConfigFileName => "gameplay_plugin.xml";

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