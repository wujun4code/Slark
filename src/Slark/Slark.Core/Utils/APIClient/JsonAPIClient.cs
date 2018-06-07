using System;
namespace Slark.Core.Utils
{
    public class JsonAPIClient : SlarkStandardAPIClient
    {
        public JsonAPIClient()
            : base(new JsonEnDecoder(), new SlarkStandardHttpClient())
        {

        }
    }
}
