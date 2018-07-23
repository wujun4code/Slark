using System;
using System.Collections.Generic;
using System.IO;

namespace TheMessage
{
    public class TMRequest
    {
        public TMRequest()
        {

        }

        public Uri Uri { get; set; }

        public virtual IList<KeyValuePair<string, string>> Headers { get; set; }

        /// <summary>
        /// Data stream to be uploaded.
        /// </summary>
        public virtual Stream Data { get; set; }

        /// <summary>
        /// HTTP method. One of <c>DELETE</c>, <c>GET</c>, <c>HEAD</c>, <c>POST</c> or <c>PUT</c>
        /// </summary>
        public string Method { get; set; }
    }
}
