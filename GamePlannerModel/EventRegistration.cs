using System.Collections.Generic;
using System.Linq;

namespace GamePlannerModel
{
    public class EventRegistration
    {
        public int ID { get; set; }
        public Event Event { get; set; }
        public User User { get; set; }
        public ICollection<Preference> Preferences { get; set; }

        public double Satisfaction { get; set; }

        private GameAssignment _assignment;
        public GameAssignment Assignment
        {
            get { return _assignment; }
            set
            {
                _assignment = value;
                if (_assignment == null)
                {
                    Satisfaction = 0;
                }
                else
                {
                    foreach (var p in Preferences)
                    {
                        if (p.Game.ID == _assignment.Game.ID)
                        {
                            Satisfaction = p.Weight;
                            break;
                        }
                    }
                }
            }
        }


        public EventRegistration() { }
        public EventRegistration(string name, ICollection<Preference> prefs)
        {
            User = new User();
            User.Name = name;
            Preferences = prefs;
        }

        public bool HasPreferenceForGame(Game game)
        {
            return Preferences.Any(p => p.Game.ID == game.ID);
        }

        public bool PrefersGameOverCurrentAssignment(Game game)
        {
            foreach (var p in Preferences)
            {
                if (p.Game.ID == game.ID)
                {
                    if (p.Weight > Satisfaction)
                        return true;

                    break;
                }
            }

            return false;
        }

        public override string ToString()
        {
            return User.Name;
        }
    }
}
