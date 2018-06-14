using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Slark.Core;
using Slark.Core.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slark.Server.LeanCloud.Play
{
    public class PlayRequest : PlayJsonMessage
    {
        public PlayRequest(string jsonMessage)
            : base(jsonMessage)
        {
            JsonObject = JObject.Parse(jsonMessage);
        }

        public JObject JsonObject
        {
            get;
        }

        public string CommandHandlerKey
        {
            get
            {
                return Body["cmd"] + "-" + Body["op"];
            }
        }

        public int CommandId
        {
            get
            {
                return int.Parse(Body["i"].ToString());
            }
        }

        public bool IsValid
        {
            get
            {
                return Body.ContainsKey("cmd") && Body.ContainsKey("op");
            }
        }

        public bool TryGet<T>(string key, T defaultValue, out T result)
        {
            var done = false;
            result = defaultValue;
            if (Body.ContainsKey(key))
            {
                try
                {
                    var temp = (T)Body[key];
                    result = temp;
                    done = true;
                }
                catch (InvalidCastException ex)
                {
                    //result = default(T);
                }
            }
            return done;
        }
    }
}
