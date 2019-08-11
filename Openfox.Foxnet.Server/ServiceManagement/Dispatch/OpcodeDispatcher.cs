using NLog;
using Openfox.Foxnet.Common.Protocol;
using Openfox.Foxnet.Server.ServiceManagement.Dispatch.PacketHandlers;
using Openfox.Foxnet.Server.ServiceManagement.Dispatch.PacketHandlers.Implementation;
using Openfox.Foxnet.Server.SocketManagement.Users.Implementation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Openfox.Foxnet.Server.ServiceManagement.Dispatch
{
    public class OpcodeDispatcher
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public static int ActiveDispatchersCount = 0;

        public static readonly Dictionary<PacketOpcode, Type> PacketHandlerLookupTable
            = new Dictionary<PacketOpcode, Type>()
            {
                {PacketOpcode.AnnounceAlive, typeof(ConnectionAnnounceHandler) },
            };

        public SocketUser User { get; }
        public NetPacket LastPacket { get; }

        public OpcodeDispatcher(SocketUser user, NetPacket lastPacket)
        {
            Interlocked.Increment(ref ActiveDispatchersCount);
            User = user;
            LastPacket = lastPacket;
        }

        public void Dispatch()
        {
            if (!PacketHandlerLookupTable.ContainsKey(LastPacket.Opcode))
                throw new NotSupportedException($"No packet handler is registered for opcode [{LastPacket.Opcode.ToString()}]");
            var handler = (IPacketHandler)Activator.CreateInstance(PacketHandlerLookupTable[LastPacket.Opcode]);
            handler.Handle(User, LastPacket);
            Logger.Info($"Finished worker thread for [{User.TcpClient.Client.RemoteEndPoint.ToString()}]");
            Interlocked.Decrement(ref ActiveDispatchersCount);
        }
    }
}
