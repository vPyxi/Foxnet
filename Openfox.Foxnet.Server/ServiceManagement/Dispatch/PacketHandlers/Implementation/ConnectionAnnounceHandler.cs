using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Openfox.Foxnet.Common.Packets;
using Openfox.Foxnet.Common.Protocol;
using Openfox.Foxnet.Common.Utility;
using Openfox.Foxnet.Server.SocketManagement.Users.Implementation;

namespace Openfox.Foxnet.Server.ServiceManagement.Dispatch.PacketHandlers.Implementation
{
    public class ConnectionAnnounceHandler : IPacketHandler
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        public async Task Handle(SocketUser user, NetPacket lastPacket)
        {
            lock (user)
            {
                if (user.State.HasHandshakedWithServer)
                {
                    Logger.Warn($"User [{user.State.Username}|{user.TcpClient.Client.RemoteEndPoint.ToString()}] is trying"
                            + " to handshake with the server even though they've already done so.");
                }
            }
            await NetData.WritePacketsAsync(user.Stream, PacketMaker.GetPacketFromOpcode(PacketOpcode.AnnounceAlive_Response));
        }

    }
}
