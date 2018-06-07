using System;
namespace Slark.Core.Utils
{
    public class SlarkGenericJsonAPIClient<T> : SlarkStandardAPIClient
    {
        public SlarkGenericJsonAPIClient()
            : base(new SlarkGenericJsonEnDecoder<T>(), new SlarkStandardHttpClient())
        {

        }
    }
}
