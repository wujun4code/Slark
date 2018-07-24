using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Slark.Core;
using Slark.Core.Protocol;

namespace TheMessage
{
    public class TMJsonRequest : ISlarkMessage
    {
        public TMJsonRequest()
        {

        }

        public TMJsonRequest(string metaText)
        {
            MetaText = metaText;
            MetaObject = MetaText.ToDictionary();

            if (MetaObject["url"] is string url)
            {
                Url = url;
            }

            if (MetaObject.ContainsKey("headers"))
            {
                if (MetaObject["headers"] is IDictionary<string, object> headers)
                    Headers = headers.ToDictionary(kv => kv.Key, kv => kv.Value.ToString()).ToList();
            }

            if (MetaObject["method"] is string method)
            {
                Method = method;
            }

            if (MetaObject["body"] is Dictionary<string, object> body)
            {
                Body = body;
            }

            if (MetaObject["i"] is int i)
            {
                CommandId = i;
            }
        }

        public IDictionary<string, object> MetaObject { get; set; }

        public string Url
        {
            get;
            set;
        }

        public IList<KeyValuePair<string, string>> Headers { get; set; }

        public IDictionary<string, object> Body { get; set; }

        public string Method { get; set; }

        public int CommandId { get; set; }

        public string MetaText { get; set; }

    }
}
