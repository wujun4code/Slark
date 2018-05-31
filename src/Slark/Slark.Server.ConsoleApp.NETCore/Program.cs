using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Slark.Server.ConsoleApp.NETCore
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .UseKestrel()
            .Build();
    }
}
