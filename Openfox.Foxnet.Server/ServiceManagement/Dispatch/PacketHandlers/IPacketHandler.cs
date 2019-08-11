using Openfox.Foxnet.Common.Protocol;
using Openfox.Foxnet.Server.SocketManagement.Users.Implementation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Openfox.Foxnet.Server.ServiceManagement.Dispatch.PacketHandlers
{
    public interface IPacketHandler
    {
        Task Handle(SocketUser user, NetPacket lastPacket);
    }
}
