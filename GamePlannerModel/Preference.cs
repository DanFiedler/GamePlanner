
namespace GamePlannerModel
{
    public class Preference
    {
        public int ID { get; set; }
        public Event Event { get; set; }
        public Game Game { get; set; }
        public double Weight { get; set; }

        public Preference() { }
        public Preference(Game game, double weight)
        {
            Game = game;
            Weight = weight;
        }
    }
}
