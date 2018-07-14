using System;
using Slark.Core;

namespace Slark.Server.LeanCloud.Play
{
    public abstract class PlayClient : SlarkClient
    {
        public PlayClient()
        {

        }

        public string PeerId { get; set; }

        public string RoomId { get; set; }
    }

    public class StandardPlayClient : PlayClient
    {

    }
}
