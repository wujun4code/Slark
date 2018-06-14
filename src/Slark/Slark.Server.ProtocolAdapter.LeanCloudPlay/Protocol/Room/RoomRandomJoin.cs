using System;
using System.Threading.Tasks;
using Slark.Core;

namespace Slark.Server.LeanCloud.Play.Protocol
{
    public class RoomRandomJoin : RoomJoin
    {
        public override string Command { get; set; } = "conv";
        public override string Operation { get; set; } = "add-random";
    }
}
