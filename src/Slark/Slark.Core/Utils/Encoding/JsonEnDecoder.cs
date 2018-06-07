using System;
namespace Slark.Core.Utils
{
    public class JsonEnDecoder : SlarkStandarkEnDecoder
    {
        public JsonEnDecoder()
            : base(new JsonEncoder(), new JsonDecoder())
        {
            
        }
    }
}
