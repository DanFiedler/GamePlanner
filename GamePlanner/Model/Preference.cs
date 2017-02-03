
namespace GamePlanner.Model
{
    public class Preference
    {
        public Game Game { get; set; }

        public double Weight { get; set; }

        public Preference() { }
        public Preference( Game game, double weight )
        {
            Game = game;
            Weight = weight;
        }
    }
}
