using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamePlannerModel
{
    public class Game
    {
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
