using System;
using System.Collections;
using System.Collections.Generic;
using LeanCloud;
using Slark.Core.Protocol;

namespace TheMessage
{
    public class TMJsonResponse : ISlarkMessage
    {
        public TMJsonResponse()
        {

        }

        public TMJsonResponse(string message)
        {
            MetaText = message;
        }

        public TMJsonResponse(string message, string cmdId)
            : this(message)
        {
            CommandId = cmdId;
        }

        public TMJsonResponse(string message, IDictionary<string, object> results, string cmdId)
            : this(message)
        {
            Results = results;
            CommandId = cmdId;
        }

        public string MetaText { get; set; }

        public IDictionary<string, object> Results { get; set; }

        public string CommandId { get; set; }
    }
}
