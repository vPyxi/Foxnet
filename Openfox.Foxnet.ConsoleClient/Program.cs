using Openfox.Foxnet.Client;
using Openfox.Foxnet.Common.Protocol;
using System;
using System.Threading;

namespace Openfox.Foxnet.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            const string ip = "127.0.0.1";
            const int port = 7800;

            Console.Title = "Foxnet Console Client";

            Thread.Sleep(2500);

            var conManager = new FoxnetConnectionManager(ip, port);
            conManager.OnServerConnected += OnServerConnected;
            conManager.Connect();

            while (true)
            {
                Thread.Sleep(2000);
            }
        }

        private static void OnServerConnected()
        {
            Console.WriteLine("Server connected.");
        }
    }
}
