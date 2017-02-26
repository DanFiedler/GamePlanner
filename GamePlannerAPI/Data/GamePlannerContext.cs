using GamePlannerModel;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace GamePlannerAPI.Data
{
    public class GamePlannerContext : DbContext
    {
        public GamePlannerContext() : base("GamePlannerContext")
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventRegistration> EventRegistrations {get;set;}
        public DbSet<Game> Games { get; set; }
        public DbSet<GameAssignment> GameAssignments { get; set; }
        public DbSet<Preference> Preferences { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}