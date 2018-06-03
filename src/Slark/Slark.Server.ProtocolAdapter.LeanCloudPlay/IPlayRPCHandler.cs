using System;
using System.Threading.Tasks;

namespace Slark.Server.LeanCloud.Play
{
    public interface IPlayRPCHandler
    {
        string Method { get; set; }

        Task<string> RPC(string message);
    }
}
