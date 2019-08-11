using Ninject;
using Openfox.Foxnet.Server.IoC;
using Openfox.Foxnet.Server.ServiceManagement;
using Openfox.Foxnet.Server.ServiceManagement.Dispatch;
using Openfox.Foxnet.Server.ServiceManagement.Implementation;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace Openfox.Foxnet.ConsoleHost
{
    /// <summary>
    /// This is mainly used as a test-bed for rapid development.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            new Thread(() => ServerThreadAction()).Start();
            new Thread(() => TitlebarStatsUpdate()).Start();

            while (true)
            {
                Thread.Sleep(250);
            }
        }

        private static void TitlebarStatsUpdate()
        {
            while (true)
            {
                var activeConnections = SocketServiceManager.ActiveConnections;
                var activeDispatchers = OpcodeDispatcher.ActiveDispatchersCount;
                Console.Title = $"FoxnetServer (Connections: {activeConnections}; Workers: {activeDispatchers})";
                Thread.Sleep(1000);
            }
        }

        public static void ServerThreadAction()
        {
            var kernel = new StandardKernel(new NinjectBindings());
            var svc = kernel.Get<ISocketServiceManager>();
            svc.Listen("127.0.0.1", 7800);
        }
    }
}
