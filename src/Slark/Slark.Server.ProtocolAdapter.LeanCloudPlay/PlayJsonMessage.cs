using System;
using System.Collections.Generic;
using System.Linq;
using Slark.Core;
using Slark.Core.Protocol;

namespace Slark.Server.LeanCloud.Play
{
    public abstract class PlayJsonMessage : ISlarkMessage
    {
        public virtual string MetaText { get; set; }
        public IDictionary<string, object> Body { get; set; }

        protected PlayJsonMessage()
        {

        }

        protected PlayJsonMessage(string jsonMessage)
        {
            MetaText = jsonMessage;
            Body = jsonMessage.ToDictionary();
        }

        protected PlayJsonMessage(IDictionary<string, object> json)
        {
            Body = json;
            MetaText = Body.ToJsonString();
        }

        public virtual PlayJsonMessage SerializeBody()
        {
            this.MetaText = this.Body.ToJsonString();
            return this;
        }

        public virtual void Timestamplize()
        {
            this.Body["serverTs"] = DateTime.Now.ToUnixTimeStamp(SlarkExtensions.UnixTimeStampUnit.Second);
        }
    }
}
