using System;
using System.Threading.Tasks;
using Slark.Core;
using Slark.Core.Utils;
namespace TheMessage.Protocols
{
    public class TMRoomCreateRequest
    {
        public async Task<string> ResponseAsync(SlarkContext context)
        {
            if (context.Server is TMLobby tmLobby)
            {
                var room = new TMRoom()
                {
                    GameMode = new TMGameMode()
                    {
                        Blue = 2,
                        Red = 2,
                        Independent = 1,
                    },
                    Id = new Guid().ToString(),
                    Players = new ConcurrentHashSet<TMPlayer>()
                    {
                        {
                            new TMPlayer(context.Sender)
                        }
                    },
                };
                context.Response = await SlarkCorePlugins.Singleton.Encoder.EncodeAsync(room);
            }
            return null;
        }
    }
}
