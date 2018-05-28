using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;

namespace Slark.ClassesDefinition
{
    public class SlarkNode
    {
        public SlarkNode()
        {
            CurrentServiceConfig = SlarkServiceConfig.Default;
        }

        public string Address { get; set; }

        public SlarkServiceConfig CurrentServiceConfig { get; set; }


        public void Start()
        {

        }
    }
}
