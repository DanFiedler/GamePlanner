using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamePlannerModel
{
    public class Game
    {
        public Game() { }
        public Game(int id, string name, int min, int max)
        {
            ID = id;
            Name = name;
            MinPlayer = min;
            MaxPlayer = max;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(450)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        public int MinPlayer { get; set; }
        public int MaxPlayer { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
