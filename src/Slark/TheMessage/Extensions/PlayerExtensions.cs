using Slark.Server.LeanCloud.Play;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TheMessage.Extensions
{
    public static class PlayerExtensions
    {
        public static Task AlloctAsync(this Player player, IEnumerable<TMCharacter> characters)
        {
            var text = JsonConvert.SerializeObject(characters);
            return player.ClientConnection.SendAsync(text);
        }
    }
}
