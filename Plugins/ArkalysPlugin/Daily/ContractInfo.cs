using System.Linq;

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

        public override string ToString() => string.Join("<br>", Objectives.Select(x => $" -  {x.Amount} * {x.Name}"));
    }
}