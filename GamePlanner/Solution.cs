using GamePlannerModel;
using System.Collections.Generic;

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
    }
}
