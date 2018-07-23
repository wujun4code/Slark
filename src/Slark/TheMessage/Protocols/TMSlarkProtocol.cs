using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slark.Core;
using Slark.Core.Protocol;

namespace TheMessage.Protocols
{
    public class TMSlarkProtocol : ISlarkProtocol
    {
        public TMSlarkProtocol(ProcessAsync process)
        {
            Process = process;
        }

        public ProcessAsync Process { get; set; }

        public Task<IEnumerable<SlarkClientConnection>> GetTargetsAsync(SlarkContext context)
        {
            return Task.FromResult(context.Receivers);
        }

        public Task<string> NotifyAsync(SlarkContext context)
        {
            return Task.FromResult(context.Notice);
        }

        public async Task<string> ResponseAsync(SlarkContext context)
        {
            await Process(context);
            return context.Response;
        }
    }
}
