using System;
using System.Collections.Generic;
using System.Text;
using Openfox.Foxnet.Common.Protocol;

namespace Openfox.Foxnet.Common.Packets.Implementations
{
    public class AnnounceAlivePacket : IPacketConstructor
    {
        public RawNetPacket[] ConstructPacket()
        {
            return new[]
            {
                new RawNetPacket
                {
                    Opcode = (int)PacketOpcode.AnnounceAlive,
                    PayloadSize = 0,
                    PayloadType = (int)PacketPayloadType.None
                }
            };
        }

        public RawNetPacket[] ConstructPacket(Dictionary<string, object> bundledData)
        {
            throw new NotSupportedException();
        }
    }
}
