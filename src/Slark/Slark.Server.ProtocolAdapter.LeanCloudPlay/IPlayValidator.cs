using Slark.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Slark.Server.LeanCloud.Play
{
    public interface IPlayValidator
    {
        Task<bool> Validate(PlayRequest playRequest);

        Func<SlarkClientConnection, PlayRequest, Task> OnInvalid { get; set; }
    }
}
