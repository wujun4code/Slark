using System;
using System.Threading.Tasks;

namespace Slark.Core.Protocol
{
    public class OkProtocol : ISlarkProtocol
    {
        public Task ExecuteAsync(SlarkContext context)
        {
            context.HasNotice = false;
            return Task.FromResult("ok");
        }
    }
}
