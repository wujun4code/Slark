using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LeanCloud;
using Slark.Core;
using Slark.Core.Protocol;

namespace TheMessage
{
    [AVClassName("TMJsonRequest")]
    public class TMJsonRequest : AVObject, ISlarkMessage
    {
        public TMJsonRequest()
        {

        }

        public TMJsonRequest(string metaText)
        {
            MetaText = metaText;
        }

        public IDictionary<string, object> MetaObject { get; set; }

        [AVFieldName("url")]
        public string Url
        {
            get { return GetProperty<string>("Url"); }
            set { SetProperty<string>(value, "Url"); }
        }

        [AVFieldName("headers")]
        public IDictionary<string, string> Headers
        {
            get { return GetProperty<IDictionary<string, string>>("Headers"); }
            set { SetProperty<IDictionary<string, string>>(value, "Headers"); }
        }

        [AVFieldName("body")]
        public IDictionary<string, object> Body
        {
            get { return GetProperty<IDictionary<string, object>>("Body"); }
            set { SetProperty<IDictionary<string, object>>(value, "Body"); }
        }

        [AVFieldName("method")]
        public string Method
        {
            get { return GetProperty<string>("Method"); }
            set { SetProperty<string>(value, "Method"); }
        }

        [AVFieldName("i")]
        public int CommandId
        {
            get { return GetProperty<int>("CommandId"); }
            set { SetProperty<int>(value, "CommandId"); }
        }

        [AVFieldName("si")]
        public string ServerCommandId
        {
            get { return GetProperty<string>("ServerCommandId"); }
            set { SetProperty<string>(value, "ServerCommandId"); }
        }

        public string MetaText { get; set; }

    }
}
