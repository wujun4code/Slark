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
    public class TMGame : AVObject
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

        public TMRoom Room { get; set; }

        public IEnumerable<TMPlayer> Players { get; set; }

        public IEquatable<TMCard> Cards { get; set; }

        [AVFieldName("mode")]
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

        public async Task InitAllotCharactersAsync(byte initCharacterCountPerPlayer = 2)
        {
            var randomCount = Players.Count() * initCharacterCountPerPlayer;
            var picked = InitCharacters.PickRandom(randomCount);
            var chunks = picked.ChunkBy(initCharacterCountPerPlayer).ToArray();
            var players = Players.ToArray();
            for (int i = 0; i < chunks.Length; i++)
            {
                await players[i].AlloctAsync(chunks[i]);
            }
        }
    }
}
