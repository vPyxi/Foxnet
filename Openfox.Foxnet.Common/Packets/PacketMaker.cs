using Openfox.Foxnet.Common.Packets.Implementations;
using Openfox.Foxnet.Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Openfox.Foxnet.Common.Packets
{
    public static class PacketMaker
    {
        private static readonly Dictionary<PacketOpcode, Type> PacketOpcodeLookupTable
            = new Dictionary<PacketOpcode, Type>()
            {
                {PacketOpcode.AnnounceAlive, typeof(AnnounceAlivePacket) },
                {PacketOpcode.AnnounceAlive_Response, typeof(AnnounceAliveResponsePacket) }
            };

        public static RawNetPacket[] GetRawPacketFromOpcode(PacketOpcode op, Dictionary<string, object> bundledData = null)
        {
            if (!PacketOpcodeLookupTable.ContainsKey(op))
                throw new NotSupportedException($"{op.ToString()} does not have an associated packet constructor.");
            var type = PacketOpcodeLookupTable[op];
            var instance = (IPacketConstructor)Activator.CreateInstance(type);
            return bundledData == null ? instance.ConstructPacket() : instance.ConstructPacket(bundledData);
        }
        public static NetPacket[] GetPacketFromOpcode(PacketOpcode op, Dictionary<string, object> bundledData = null)
            => NetPacket.FromRawPacket(GetRawPacketFromOpcode(op, bundledData));
    }
}
