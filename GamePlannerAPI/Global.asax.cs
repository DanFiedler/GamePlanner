using GamePlannerAPI.Data;
using System.Data.Entity;
using System.Web;
using System.Web.Http;

namespace GamePlannerAPI
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            Database.SetInitializer(new DbInitializer());
            using (var context = new GamePlannerContext())
            {
                context.Database.Initialize(false);
            }
        }
    }
}
