using System;
using System.Threading.Tasks;

namespace Slark.Server.LeanCloud.Play
{
    public interface IPlayRouteHandler
    {
        string Router { get; set; }

        Task<string> Response(string message);
    }
}
