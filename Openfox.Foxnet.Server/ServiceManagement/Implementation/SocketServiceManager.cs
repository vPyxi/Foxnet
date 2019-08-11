using NLog;
using Openfox.Foxnet.Common.Packets;
using Openfox.Foxnet.Common.Protocol;
using Openfox.Foxnet.Common.Utility;
using Openfox.Foxnet.Server.ServiceManagement;
using Openfox.Foxnet.Server.ServiceManagement.Dispatch;
using Openfox.Foxnet.Server.SocketManagement.Authentication;
using Openfox.Foxnet.Server.SocketManagement.Users;
using Openfox.Foxnet.Server.SocketManagement.Users.Implementation;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Openfox.Foxnet.Server.ServiceManagement.Implementation
{
    public class SocketServiceManager : ISocketServiceManager
    {
        public bool IsRunning { get; set; }
        private static int _activeConnections;
        public static int ActiveConnections { get => _activeConnections; }
        public IPAddress IPAddress { get; set; }
        public int Port { get; set; }
        public Exception ServerException { get; set; }
        public IUserAuthHandler UserAuthHandler { get; }

        private readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public SocketServiceManager(IUserAuthHandler userAuthHandler)
        {
            UserAuthHandler = userAuthHandler;
        }

        public async Task Listen() => await Listen(IPAddress.ToString(), Port);
        public async Task Listen(string ip, int port)
        {
            if (port <= 0 || port >= short.MaxValue)
                throw new ArgumentOutOfRangeException($"{nameof(port)} must be within 1-{short.MaxValue}");
            if (string.IsNullOrWhiteSpace(ip))
                throw new ArgumentNullException($"{nameof(ip)} must not be null or empty.");
            if (!IPAddress.TryParse(ip, out var addr))
                throw new ArgumentException($"{nameof(ip)} is not a valid IP Address.");

            Port = port;
            IPAddress = addr;
            Logger.Info($"Spinning up service manager up on {IPAddress.ToString()}:{Port}...");

            try
            {
                var tcpListener = new TcpListener(addr, port);
                tcpListener.Start();

                Logger.Info($"Server is actively listening for connections.");

                IsRunning = true;
                
                while (true)
                {
                    Logger.Info("Waiting for an incoming connection.");

                    var client = tcpListener.AcceptTcpClient();

                    Interlocked.Increment(ref _activeConnections);
                    Logger.Info($"Accepted a connection from [{client.Client.RemoteEndPoint.ToString()}]");

                    new Thread(async () =>
                    {
                        try
                        {
                            var stream = client.GetStream();

                            var socketUser = new SocketUser
                            {
                                TcpClient = client,
                            };

                            while (true)
                            {
                                while (!stream.DataAvailable)
                                {
                                    Thread.Sleep(25);
                                }
                                var packet = await NetData.ReadPacketAsync(stream, client.ReceiveBufferSize);
                                Logger.Info("Creating dispatch worker thread for request...");
                                new Thread(() => new OpcodeDispatcher(socketUser, packet).Dispatch()).Start();
                            }
                        }
                        catch (Exception e)
                        {
                            Logger.Error(e, $"Failed while handling a connection from {client.Client.RemoteEndPoint.ToString()}");
                            Interlocked.Decrement(ref _activeConnections);
                        }
                    }).Start();
                }
            }
            catch (Exception e)
            {
                IsRunning = false;
                ServerException = e;
                throw;
            }
        }
    }
}
