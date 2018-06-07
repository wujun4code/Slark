using System;
namespace Slark.Core.Utils
{
    public class JsonPollingController : SlarkStandardPollingController
    {
        public JsonPollingController(string apiUrl, uint dueTimeInSecond, uint intervalInSecond)
            : base(apiUrl, new JsonAPIClient(), dueTimeInSecond, intervalInSecond)
        {

        }
    }
}
