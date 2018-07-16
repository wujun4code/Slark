using System;
using Slark.Core;

namespace Slark.Server.LeanCloud.Play
{
    public class PlayClientInfo
    {
        public string DeviceName { get; set; }

        public string OS { get; set; }

        public string OSVersion { get; set; }

        public string DeviceBrand { get; set; }
    }

    public abstract class PlayClient : SlarkClient
    {
        public PlayClient()
        {

        }

        public string PeerId { get; set; }

        public string RoomId { get; set; }

        public PlayClientInfo Info { get; set; }
    }

    public sealed class StandardPlayClient : PlayClient
    {

    }
}
