using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Slark.Core.Utils
{
    public interface ISlarkEncoder
    {
        Task<string> EncodeAsync(object obj);
        string Encode(object obj);
    }
}