using GamePlannerModel;

namespace GamePlannerAPI.Data
{
    public class DbInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<GamePlannerContext>
    {
        protected override void Seed(GamePlannerContext context)
        {
            bool shouldCreate = true;

            if (context.Database.Exists())
            {
                if (context.Database.CompatibleWithModel(true))
                {
                    shouldCreate = false;
                }
                else
                {
                    context.Database.Delete();
                }
            }

            if (shouldCreate)
            {
                context.Database.Create();
                context.Games.Add(new Game() { MinPlayer = 3, MaxPlayer = 5, Name = "Settlers of Catan" });
                context.SaveChanges();
            }
        }
    }
}