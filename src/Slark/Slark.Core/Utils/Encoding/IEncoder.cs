using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Slark.Core.Utils
{
    public interface ISlarkEncoder
    {
        Task<string> Encode(object obj);
    }
}