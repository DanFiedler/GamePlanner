using System;
using System.Collections.Generic;
using System.Linq;

namespace GamePlanner.Model
{
    public class Assignment
    {
        public Assignment(Game game)
        {
            Game = game;
            Players = new List<Player>();
        }

        public Game Game { get; set; }
        public List<Player> Players { get; set; }

        private double _totalSatisfaction;
        public double TotalSatisfaction
        {
            get
            {
                if (_totalSatisfaction == 0)
                    _totalSatisfaction = Players.Sum(p => p.Satisfaction);

                return _totalSatisfaction;
            }
        }

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
            for(int i = 0; i < Players.Count; i++)
            {
                if (i != 0)
                    sb.Append(",");

                sb.Append(Players[i].Name);
            }
            sb.Append("}");
            return sb.ToString();
        }
    }
}
