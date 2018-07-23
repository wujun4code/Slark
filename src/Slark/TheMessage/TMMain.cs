using System;
using Slark.Core;

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
