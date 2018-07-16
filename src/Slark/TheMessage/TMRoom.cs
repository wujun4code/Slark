using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slark.Server.LeanCloud.Play;
using Slark.Server.LeanCloud.Play.Protocol;
using Slark.Core.Extensions;
using System.Linq;
using TheMessage.Extensions;

namespace TheMessage
{
    public class TMGameMode
    {
        public byte Total { get; set; }

        public byte Blue { get; set; }

        public byte Red { get; set; }

        public byte Independent { get; set; }
    }


    public class TMRoom : PlayRoom
    {
        public TMRoom()
        {

        }

        //public enum GameMode
        //{
        //    R1B1I1T3 = 1,
        //    R1B1I2T4,
        //    R2B2I0T4,
        //    R2B2I1T5,
        //    R2B2I2T6,
        //    R3B3I0T6,
        //    R3B3I1T7,
        //    R2B2I3T7,
        //    R3B3I2T8,
        //    R3B3I3T9
        //}

        public TMGameMode GameMode { get; set; }
        public IEnumerable<TMCharacter> InitCharacters { get; set; }
        public async Task InitAllotCharactersAsync(byte initCharacterCountPerPlayer = 2)
        {
            var randomCount = Players.Count * initCharacterCountPerPlayer;
            var picked = InitCharacters.PickRandom(randomCount);
            var chunks = picked.ChunkBy(initCharacterCountPerPlayer).ToArray();
            var players = Players.ToArray();
            for (int i = 0; i < chunks.Length; i++)
            {
                await players[i].SendAsync(chunks[i]);
            }
        }

        //public Task InitAllotCampsAsync(IEnumerable<TMPlayer> blues, IEnumerable<TMPlayer> reds, IEnumerable<TMPlayer> independents)
        //{
        //    //if (roomMode.Total != Players.Count) return Task.FromException(new Exception("mode not macth player's count."));
        //}

        //public Task InitAllotCardsAsync(IEnumerable<TMPlayer> blues, IEnumerable<TMPlayer> reds, IEnumerable<TMPlayer> independents)
        //{

        //}


        public TMRoom(RoomConfig config) : base(config)
        {

        }
    }
}
