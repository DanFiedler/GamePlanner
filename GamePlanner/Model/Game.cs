
namespace GamePlanner.Model
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MinPlayer { get; set; }
        public int MaxPlayer { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
