﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Slark.Core.Protocol;

namespace Slark.Server.LeanCloud.Play
{
    public interface IPlayCommandHandler : ISlarkProtocol
    {
        string Command { get; set; }
        string Operation { get; set; }

        Task ExecuteAsync(PlayContext context);
    }
}
