using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeanCloud;
using Slark.Core;
using TheMessage.Extensions;

namespace TheMessage
{
    public static class TM
    {
        public static void Init()
        {
            SetupPlugins();
        }


        public static void SetupPlugins()
        {
            SlarkCorePlugins.Singleton.Decoder = new LeanCloudJsonDecoder();
            SlarkCorePlugins.Singleton.Encoder = new LeanCloudJsonEncoder();
        }
    }
}
