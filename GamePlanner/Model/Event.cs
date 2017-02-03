using System.Collections.Generic;

namespace GamePlanner.Model
{
    public class Event
    {
        public List<Player> Players { get; set; }
        public List<Game> GameOptions { get; set; }

        public int NumberOfRounds { get; set; }

        public Event()
        {
            NumberOfRounds = 1;
        }
    }
}
