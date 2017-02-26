using System.Collections.Generic;
using System.Linq;

namespace GamePlannerModel
{
    public class GameAssignment
    {
        public int ID { get; set; }
        public Event Event { get; set; }
        public Game Game { get; set; }
        public ICollection<EventRegistration> Players {get;set;}


        public bool IsValid()
        {
            if (Players.Count < Game.MinPlayer)
                return false;

            if (Players.Count > Game.MaxPlayer)
                return false;

            return true;
        }

        public bool CanAddPlayer()
        {
            return Players.Count < Game.MaxPlayer;
        }

        public bool CanLosePlayer()
        {
            return Players.Count > Game.MinPlayer;
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder(Game.Name);
            sb.Append("{");

            int i = 0;
            foreach( var player in Players )
            {
                if (i != 0)
                    sb.Append(",");

                sb.Append(player.User.Name);
                i++;
            }

            sb.Append("}");
            return sb.ToString();
        }

        private double _totalSatisfaction;
        public double GetTotalSatisfaction()
        {
            if (_totalSatisfaction == 0)
                _totalSatisfaction = Players.Sum(p => p.Satisfaction);

            return _totalSatisfaction;
        }
    }
}
