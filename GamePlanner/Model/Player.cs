using System.Collections.Generic;
using System.Linq;

namespace GamePlanner.Model
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public List<Preference> Preferences { get; set; }
        public double Satisfaction { get; private set; }

        private Assignment _assignment;
        public Assignment Assignment
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
                        if (p.Game.Id == _assignment.Game.Id)
                        {
                            Satisfaction = p.Weight;
                            break;
                        }
                    }
                }
            }
        }
        public Player(int id, string name)
        {
            Id = id;
            Name = name;
            Preferences = new List<Preference>();
        }

        public Player(int id, string name, List<Preference> preferences)
        {
            Id = id;
            Name = name;
            Preferences = preferences;
        }

        public bool HasPreferenceForGame(Game game)
        {
            return Preferences.Any(p => p.Game.Id == game.Id);
        }

        public bool PrefersGameOverCurrentAssignment(Game game)
        {
            foreach( var p in Preferences )
            {
                if( p.Game.Id == game.Id )
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
            return Name;
        }
    }
}
