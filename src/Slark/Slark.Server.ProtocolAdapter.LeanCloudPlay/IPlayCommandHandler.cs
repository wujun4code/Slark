using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Slark.Server.LeanCloud.Play
{
    public interface IPlayCommandHandler
    {
        string Command { get; set; }
        string Operation { get; set; }

        Task ExecuteAsync(PlayContext context);
    }
}
