using Openfox.Foxnet.Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Openfox.Foxnet.Common.Packets
{
    public interface IPacketConstructor
    {
        RawNetPacket[] ConstructPacket();
        RawNetPacket[] ConstructPacket(Dictionary<string, object> bundledData);
    }
}
