using System;
using System.Collections.Generic;

namespace Slark.Core
{
    public class SlarkCommand
    {
        public SlarkCommand()
        {

        }

        public static SlarkCommand Received(string obj)
        {
            return new SlarkCommand()
            {
                Body = obj.ToDictionary()
            };
        }
        public IDictionary<string, object> Body { get; set; }
    }
}
