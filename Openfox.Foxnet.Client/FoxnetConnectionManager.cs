using NLog;
using Openfox.Foxnet.Common.Packets;
using Openfox.Foxnet.Common.Protocol;
using Openfox.Foxnet.Common.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Openfox.Foxnet.Client
{
    public class FoxnetConnectionManager
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private Thread ServerListenThread;

        public string Hostname { get; set; }
        public int Port { get; set; }

        public delegate void ServerEventHandler(RawNetPacket packet, byte[] buffer);
        public delegate void ServerBasicEventHandler();

        public event ServerBasicEventHandler OnServerConnected;

        public FoxnetConnectionManager(string hostname, int port)
        {
            Hostname = hostname;
            Port = port;
        }

        public void Connect()
        {
            if (ServerListenThread?.IsAlive == true)
            {
                Logger.Error($"Server connection is already alive, but {nameof(FoxnetConnectionManager)}.{nameof(Connect)}() was called");
                return;
            }

            ServerListenThread = new Thread(async () => await ServerListenThreadAction(Hostname, Port));
            ServerListenThread.Start();
        }

        public void Disconnect()
        {
            if (ServerListenThread?.IsAlive == true)
            {
                ServerListenThread.Interrupt();
            }
        }

        public async Task ServerListenThreadAction(string hostname, int port)
        {
            var client = default(TcpClient);

            try
            {
                client = new TcpClient(hostname, port);
                Logger.Info($"Connected to [{hostname}:{port}]");

                var stream = client.GetStream();

                Logger.Info($"Sending [{nameof(PacketOpcode.AnnounceAlive)}] packet to server.");
                var announceAlive = PacketMaker.GetPacketFromOpcode(PacketOpcode.AnnounceAlive);
                await NetData.WritePacketsAsync(stream, announceAlive);

                Logger.Info($"Waiting on response...");
                var response = await NetData.ReadPacketAsync(stream, client.ReceiveBufferSize);

                if (response.Opcode == PacketOpcode.AnnounceAlive_Response)
                {
                    Logger.Info($"Received valid {nameof(PacketOpcode.AnnounceAlive_Response)} packet from server.");
                    OnServerConnected?.Invoke();
                }
                else
                {
                    Logger.Error($"Failed handshake with server. Expected [{nameof(PacketOpcode.AnnounceAlive_Response)}] but received [{response.Opcode}] instead.");
                }
            }
            catch (ThreadInterruptedException e)
            {
                Logger.Error($"{nameof(ServerListenThreadAction)} thread was interrupted and connection will be forcibly closed by us.");
            }
            finally
            {
                if (client != null && client.Connected)
                {
                    client.Close();
                }
            }
        }
    }
}
