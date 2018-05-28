using System;
using Slark.ClassesDefinition;
using System.Collections.Generic;

namespace Slark.Server.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            SlarkNode node = new SlarkNode();
            node.Start();
            //SlarkLobby lobby = new SlarkLobby();
            //lobby.Start();
            Console.WriteLine("Hello World!");
        }
    }
}

