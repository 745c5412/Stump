using System;
using System.Linq;
using System.Text;
using Stump.Server.WorldServer.Database.Monsters;

namespace ArkalysPlugin.Daily
{
    public class ContractInfo
    {

        public ContractInfo(DailyObjectiveRecord[] objectives)
        {
            Objectives = objectives;
        }
        
        public DailyObjectiveRecord[] Objectives
        {
            get;
        }
        
        public override string ToString()
        {
            return string.Join("\r\n", Objectives.Select(x => $" -  {x.Amount} * {x.Name}"));
        }
    }
}