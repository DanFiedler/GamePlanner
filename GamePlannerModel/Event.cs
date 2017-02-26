using System;
using System.Collections.Generic;

namespace GamePlannerModel
{
    public class Event
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }

        public ICollection<Game> GameOptions { get; set; }
        
        public ICollection<EventRegistration> Players { get; set; }


        public string WorkLog { get; set; }
    }
}
