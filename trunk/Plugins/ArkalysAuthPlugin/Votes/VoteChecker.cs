﻿using System;
using System.Linq;
using System.Threading;
using Stump.Core.Reflection;
using Stump.Server.AuthServer;
using Stump.Server.AuthServer.Database;
using Stump.Server.AuthServer.Managers;
using Stump.Server.BaseServer.Initialization;

namespace ArkalysAuthPlugin.Votes
{
    public class VoteChecker : Singleton<VoteChecker>
    {
        [Initialization(InitializationPass.Last)]
        public void Initialize()
        {
            AuthServer.Instance.IOTaskPool.CallPeriodically(10000, CheckVotes);
        }

        private static void CheckVotes()
        {
            var votes =
                AuthServer.Instance.DBAccessor.Database.Query<Account>(
                    string.Format(
                        "SELECT Id,LastConnectionWorld FROM `accounts` WHERE `LastConnectionWorld` IS NOT NULL AND `LastVote` IS NULL OR `LastVote` < '{0}'",
                        (DateTime.Now - TimeSpan.FromHours(3)).ToString("yyyy-MM-dd hh:mm:ss")));

            var groupedAccounts = from vote in votes
                                  group vote by vote.LastConnectionWorld;

            foreach (var group in groupedAccounts)
            {
                if (group.Key == null)
                    continue;
         
                var world = WorldServerManager.Instance.GetServerById((int)group.Key);

                if (world.IPCClient == null)
                    continue;

                var total = group.Count();
                for (var i = 0; i <= total; i += 2000)
                {
                    var list = group.Skip(i).Take(2000);

                    var send = list.Select(x => x.Id).ToArray();
                    world.IPCClient.Send(new VoteNotificationMessage(send));

                    //Very Ugly
                    Thread.Sleep(1000);
                }
            }
        }
    }
}