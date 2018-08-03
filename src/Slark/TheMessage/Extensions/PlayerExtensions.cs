using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Slark.Core;

namespace TheMessage.Extensions
{
    public static class PlayerExtensions
    {
        public static Task AlloctAsync(this TMPlayer player, IEnumerable<TMCharacter> characters)
        {
            var text = SlarkCorePlugins.Singleton.Encoder.Encode(characters);
            return player.Client.SendAsync(text);
        }
    }
}
