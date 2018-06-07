using System;
namespace Slark.Core.Utils
{
    public class SlarkGenericJsonPollingController<T> : SlarkStandardPollingController
    {
        public SlarkGenericJsonPollingController(string apiUrl, uint dueTimeInSecond, uint intervalInSecond)
            : base(apiUrl, new SlarkGenericJsonAPIClient<T>(), dueTimeInSecond, intervalInSecond)
        {

        }
    }
}
