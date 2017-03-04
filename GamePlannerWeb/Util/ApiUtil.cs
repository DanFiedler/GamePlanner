using Microsoft.Rest;

namespace GamePlannerWeb.Util
{
    public static class ApiUtil
    {
        public static IGamePlannerWebClient GetClient()
        {
            return new GamePlannerWebClient(
                new TokenCredentials("foo")
            );
        }
    }
}