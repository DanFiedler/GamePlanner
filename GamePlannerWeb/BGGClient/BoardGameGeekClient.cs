using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using GamePlannerWeb.Models;

namespace GamePlannerWeb.BGGClient
{
    public class BggApiResult
    {
        public bool Success { get; set; }
        public string Error { get; set; }

        public string UserName { get; set; }
        public List<int> Games { get; set; }
    }

    public class BoardGameGeekClient
    {
        private const string COLLECTION_API = "https://boardgamegeek.com/xmlapi2/collection?subtype=boardgame&own=1&username=";
        private const string THING_API = "https://boardgamegeek.com/xmlapi2/thing?id=";

        public BggApiResult GetGamesForUser(string userName)
        {
            var result = new BggApiResult() { UserName = userName };

            using (var httpClient = new HttpClient())
            {
                var userCollectionResponseTask = httpClient.GetAsync(COLLECTION_API + userName);

                var userCollectionResponse = userCollectionResponseTask.Result;
                
                if( userCollectionResponse.IsSuccessStatusCode )
                {
                    string userCollectionXml = GetContentAsString(userCollectionResponse.Content);
                    result.Games = GetGameIdsForUserCollection(userCollectionXml);
                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                    result.Error = userCollectionResponse.StatusCode + ": " + userCollectionResponse.ReasonPhrase;
                }
            }
            
            return result;
        }

        private string GetContentAsString( HttpContent httpContent )
        {
            var bytes = httpContent.ReadAsByteArrayAsync().Result;
            string xml = System.Text.Encoding.UTF8.GetString(bytes);
            // BGG doesn't properly xml encode some strings like "Tigris & Euphrates"
            return xml.Replace(" & ", " &amp; ");
        }

        private List<int> GetGameIdsForUserCollection(string userCollectionXml)
        {
            var gameIds = new List<int>();

            var serializer = new XmlSerializer(typeof(BggCollection.Items));
            using (var reader = new StringReader(userCollectionXml))
            {
                using (var xmlReader = new XmlTextReader(reader))
                {
                    xmlReader.XmlResolver = null;
                    xmlReader.DtdProcessing = DtdProcessing.Prohibit;
                    BggCollection.Items collection = serializer.Deserialize(xmlReader) as BggCollection.Items;

                    foreach(BggCollection.Item item in collection.Item )
                    {
                        int id;
                        if( int.TryParse( item.Objectid, out id ))
                        {
                            gameIds.Add(id);
                        }
                    }
                }
            }

            return gameIds;
        }

        public List<Game> GetDetailedGameList(List<int> gameIds)
        {
            using (var httpClient = new HttpClient())
            {
                var gameList = new List<Game>();
                foreach (int id in gameIds)
                {
                    Game game = GetDetailsForGame(httpClient, id);
                    if (game != null)
                        gameList.Add(game);
                }
                return gameList;
            }
        }

        private Game GetDetailsForGame(HttpClient httpClient, int id)
        {
            try
            {
                var response = httpClient.GetAsync(THING_API + id).Result;
                string thingXml = GetContentAsString(response.Content);

                var serializer = new XmlSerializer(typeof(BggThing.Items));
                using (var reader = new StringReader(thingXml))
                {
                    using (var xmlReader = new XmlTextReader(reader))
                    {
                        xmlReader.XmlResolver = null;
                        xmlReader.DtdProcessing = DtdProcessing.Prohibit;
                        BggThing.Items collection = serializer.Deserialize(xmlReader) as BggThing.Items;

                        string gameName = GetGameName(collection.Item.Name);
                        int minPlayers = 0;
                        if( !string.IsNullOrEmpty(collection.Item.Minplayers.Value))
                        {
                            int.TryParse(collection.Item.Minplayers.Value, out minPlayers);
                        }

                        int maxPlayers = 0;
                        if(!string.IsNullOrEmpty(collection.Item.Maxplayers.Value))
                        {
                            int.TryParse(collection.Item.Maxplayers.Value, out maxPlayers);
                        }

                        if (minPlayers > 0 && maxPlayers > 0 && !string.IsNullOrEmpty(gameName))
                        {
                            return new Game()
                            {
                                Name = gameName,
                                MinPlayer = minPlayers,
                                MaxPlayer = maxPlayers
                            };
                        }
                    }
                }
            }
            catch
            { }

            return null;
        }

        private string GetGameName(List<BggThing.Name> names)
        {
            string gameName = null;

            foreach( var name in names )
            {
                // <name type="primary" sortindex="1" value="Dominion: Prosperity"/>
                if (string.Compare(name.Type, "primary", true) == 0)
                    gameName = name.Value;
            }

            if (string.IsNullOrEmpty(gameName) && names.Count > 0)
                return names[0].Value;

            return gameName;
        }
    }
}