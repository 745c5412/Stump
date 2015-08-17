using NLog;
using Stump.Core.Pool;
using Stump.Server.BaseServer;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stump.Plugins.DefaultPlugin.Memory
{
    public static class BufferLeaksFix
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [Initialization(InitializationPass.Any, Silent = true)]
        public static void Initialize()
        {
            WorldServer.Instance.IOTaskPool.CallPeriodically((int)TimeSpan.FromMinutes(10).TotalMilliseconds, CheckForLeaks);
        }

        public static void CheckForLeaks()
        {
            int leaks = BufferManager.Managers.Sum(x => x.CheckForLeaks());

            if (leaks > 0)
                logger.Warn("{0} leaked segment removed", leaks);
        }
    }
}
