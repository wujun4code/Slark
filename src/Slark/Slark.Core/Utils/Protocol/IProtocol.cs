using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Slark.Core.Protocol
{
    public interface ISlarkProtocol
    {
        Task ExecuteAsync(SlarkContext context);
    }
}
