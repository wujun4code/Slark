using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Slark.Core;

namespace Slark.Server.LeanCloud.Play.Protocol.Validator
{
    public class EmptyDataPacket : IPlayValidator
    {
        public EmptyDataPacket()
        {
            OnInvalid = async (connection, request) =>
             {
                 await connection.SendAsync(request.MetaString);
             };
        }
        public Func<SlarkClientConnection, PlayRequest, Task> OnInvalid { get; set; }

        public Task<bool> Validate(PlayRequest playRequest)
        {
            return Task.FromResult(playRequest.MetaString != "{}");
        }
    }
}
