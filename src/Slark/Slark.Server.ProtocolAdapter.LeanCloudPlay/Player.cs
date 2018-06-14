using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Slark.Core;

namespace Slark.Server.LeanCloud.Play
{
    public class Player
    {
        public PlayClient Client
        {
            get
            {
                return ClientConnection.Client as PlayClient;
            }
        }

        public SlarkClientConnection ClientConnection { get; set; }

        public uint ActorId { get; set; }

        public string PeerId
        {
            get
            {
                return Client.PeerId;
            }
        }

        public IDictionary<string, object> CustomProperties { get; set; }
    }

    public static class PlayerExtensions
    {
        public static IDictionary<string, object> ToJsonDictionary(this Player player)
        {
            var result = new Dictionary<string, object>
            {
                { "pid", player.PeerId },
                { "actorId", player.ActorId },
                { "properties", player.CustomProperties }
            };
            return result;
        }
    }
}
