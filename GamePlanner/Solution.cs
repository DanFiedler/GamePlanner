using GamePlannerModel;
using System.Collections.Generic;
using System.Text;

namespace GamePlanner
{
    public class Solution
    {
        public double MeanSatisfaction { get; set; }
        public double AverageSatisfaction { get; set; }
        public double TotalSatisfaction { get; set; }

        public List<GameAssignment> Assignments { get;set; }

        public Solution()
        {
            Assignments = new List<GameAssignment>();
        }

        public bool IsValid()
        {
            foreach( var assignment in Assignments)
            {
                if( !assignment.IsValid() )
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("Solution Total Satisfaction:{0} Avg:{1} Mean:{2} {3}", TotalSatisfaction, AverageSatisfaction, MeanSatisfaction, System.Environment.NewLine);
            foreach( var assignment in Assignments )
            {
                sb.AppendLine(assignment.ToString());
            }
            sb.AppendLine("--------------------");
            return sb.ToString();
        }
    }
}
