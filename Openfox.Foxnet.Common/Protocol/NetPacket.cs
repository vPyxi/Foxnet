using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Openfox.Foxnet.Common.Protocol
{
    public class NetPacket
    {
        public static NetPacket FromRawPacket(RawNetPacket packet)
        {
            return new NetPacket
            {
                Opcode = (PacketOpcode)packet.Opcode,
                PayloadType = (PacketPayloadType)packet.PayloadType,
                PayloadSize = packet.PayloadSize
            };
        }

        public static NetPacket[] FromRawPacket(RawNetPacket[] rawNetPacket)
        {
            return rawNetPacket.Select(p => NetPacket.FromRawPacket(p)).ToArray();
        }

        public RawNetPacket ToRawPacket()
        {
            return new RawNetPacket
            {
                Opcode = (byte)Opcode,
                PayloadType = (byte)PayloadType,
                PayloadSize = (byte)PayloadSize,
            };
        }

        public PacketOpcode Opcode { get; set; }
        public PacketPayloadType PayloadType { get; set; }
        public int PayloadSize { get; set; }
    }
}
