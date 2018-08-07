using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeanCloud;
using TheMessage.Extensions;
using Slark.Core.Extensions;

namespace TheMessage
{
    public struct TMGameMode
    {
        public byte Total
        {
            get => (byte)(Blue + Red + Independent);
        }

        public byte Blue { get; set; }

        public byte Red { get; set; }

        public byte Independent { get; set; }

        public TMGameMode From(IDictionary<string, byte> dict)
        {
            Red = dict["r"];
            Blue = dict["b"];
            Independent = dict["i"];
            return this;
        }

        public IDictionary<string, byte> To()
        {
            return new Dictionary<string, byte>()
            {
                { "r", Red },
                { "b", Blue },
                { "i", Independent },
            };
        }
    }

    [AVClassName("TMGame")]
    public class TMGame : AVObject, IRpc
    {
        public TMGame()
        {

        }

        [RpcHostIdProperty]
        public string Id
        {
            get
            {
                return this.ObjectId;
            }
        }

        [AVFieldName("room")]
        public TMRoom Room 
        { 
            get
            {
                return this.GetProperty<TMRoom>("Room");
            }
            set
            {
                this.SetProperty<TMRoom>(value, "Room");
            }
        }

        //[AVFieldName("players")]
        //public IList<TMPlayer> Players
        //{ 
        //    get
        //    {
        //        return this.GetProperty<IList<TMPlayer>>("Players");
        //    }
        //    set
        //    {
        //        this.SetProperty<IList<TMPlayer>>(value, "Players");
        //    }
        //}

        public IList<TMPlayer> Players
        {
            get;set;
        }

        public IEquatable<TMCard> Cards { get; set; }


        public TMGameMode GameMode
        {
            get
            {
                var dict = this.GetProperty<IDictionary<string, byte>>("GameMode");
                return new TMGameMode().From(dict);
            }
            set
            {
                var dict = value.To();
                this.SetProperty<IDictionary<string, byte>>(dict, "GameMode");
            }
        }

        public void AdjustMode()
        {
            var playerCount = Players.Count();
            switch (playerCount)
            {
                case 4:
                    {
                        GameMode = new TMGameMode()
                        {
                            Blue = 1,
                            Red = 1,
                            Independent = 2
                        };
                        break;
                    }
                case 5:
                    {
                        GameMode = new TMGameMode()
                        {
                            Blue = 2,
                            Red = 2,
                            Independent = 1
                        };
                        break;
                    }
                case 6:
                    {
                        GameMode = new TMGameMode()
                        {
                            Blue = 2,
                            Red = 2,
                            Independent = 2
                        };
                        break;
                    }
                case 7:
                    {
                        GameMode = new TMGameMode()
                        {
                            Blue = 3,
                            Red = 3,
                            Independent = 1
                        };
                        break;
                    }
            }
        }

        public Task StartAsync(TMGameMode gameMode)
        {
            if (Players.Count() != gameMode.Total) return Task.FromException(new TMException(4001, "房间人数与牌局要求人数不一致"));
            this.GameMode = gameMode;
            return Task.FromResult(true);
        }

        public IEnumerable<TMCharacter> InitCharacters { get; set; }

        public async Task SavePlayersAsync(TMRoom room)
        {
            var players = new List<TMPlayer>();
            foreach (var info in room.ClientInfos)
            {
                TMPlayer player = new TMPlayer(info) { Game = this };
                players.Add(player);
            }
            Players = players;
            //this.AdjustMode();
            await this.SaveAsync();
        }

        public async Task InitAllotCharactersAsync(byte initCharacterCountPerPlayer = 2)
        {
            InitCharacters = await new AVQuery<TMCharacter>().Limit(100).Select("screenName").Select("serial").FindAsync();
            var randomCount = Players.Count() * initCharacterCountPerPlayer;
            var picked = InitCharacters.PickRandom(randomCount);
            var chunks = picked.ChunkBy(initCharacterCountPerPlayer).ToArray();
            var players = Players.ToArray();

            List<Task> all = new List<Task>();

            for (int i = 0; i < chunks.Length; i++)
            {
                var characters = chunks[i];
                var player = players[i];
                var task = player.RpcWithChoiceAsync("OnCharactersAllotted", characters.ToList()).ContinueWith(t =>
                  {
                      TMCharacter selectdCharacter = t.Result;
                      return this.RpcAllAsync("OnPlayerSelectedCharacter", player, selectdCharacter);
                  }).Unwrap();

                all.Add(task);
            }
            await Task.WhenAll(all);
            await this.RpcAllAsync("OnAllCharactersSelected");
        }
    }
}
