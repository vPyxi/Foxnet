using NLog;
using Openfox.Foxnet.Server.ServiceManagement;
using Openfox.Foxnet.Server.ServiceManagement.Implementation;
using Openfox.Foxnet.Server.SocketManagement.Authentication.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Openfox.Foxnet.Web.Services
{
    public static class ServiceManager
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private static object SyncObject = new object();
        public static Thread ListenThread;

        public static void Listen(int port)
        {
            lock (SyncObject)
            {
                Logger.Info("Acquired mutex lock for server listen thread");
                var socketManager = new SocketServiceManager(new UsernameOnlyAuthHandler())
                {
                    IPAddress = IPAddress.Parse("127.0.0.1"),
                    Port = port
                };

                ListenThread = new Thread(() => socketManager.Listen());
                ListenThread.Start();
            }
        }
    }
}
