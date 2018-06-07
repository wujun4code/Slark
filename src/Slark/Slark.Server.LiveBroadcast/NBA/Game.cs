using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Slark.Core.Utils;

namespace Slark.Server.LiveBroadcast.NBA
{
    public class Game : ISlarkDecoder
    {
        public Game()
        {

        }

        [JsonProperty("gameId")]
        public string GameId { get; set; }

        [JsonProperty("vTeam")]
        public Team Visiting { get; set; }

        [JsonProperty("hTeam")]
        public Team Host { get; set; }

        [JsonProperty("isGameActivated")]
        public bool IsGameActivated { get; set; }

        public Task<object> Decode(string message)
        {
            JObject o = JObject.Parse(message);
            JArray gamesMetaData = o["games"] as JArray;
            IList<Game> games = gamesMetaData.Select(p => JsonConvert.DeserializeObject<Game>(p.ToString())).ToList();
            return Task.FromResult(games as object);
        }

        public override string ToString()
        {
            var obj = new
            {
                vtc = new
                {
                    tc = Visiting.TribleCode,
                    score = Visiting.Score,
                },
                htc = new
                {
                    tc = Host.TribleCode,
                    score = Host.Score,
                },
            };
            return JsonConvert.SerializeObject(obj);
        }
    }
}
