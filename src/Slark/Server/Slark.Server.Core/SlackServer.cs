using System;
using System.Collections.Generic;
using System.Text;

namespace Slark.Server.Core
{
    public abstract class SlackServer
    {
        IConnectionManager ConnectionManager { get; set; }
    }
}
