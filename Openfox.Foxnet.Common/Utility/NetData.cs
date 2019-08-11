using Openfox.Foxnet.Common.Protocol;
using Openfox.Foxnet.Common.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Openfox.Foxnet.Common.Utility
{
    public static class NetData
    {
        public static async Task WritePacketsAsync(NetworkStream stream, NetPacket[] packets)
            => await WritePacketsAsync(stream, packets.Select(p => p.ToRawPacket()).ToArray());
        public static async Task WritePacketsAsync(NetworkStream stream, RawNetPacket[] packets)
        {
            foreach (var p in packets)
                await WritePacketAsync(stream, p);
        }

        public static async Task WritePacketAsync(NetworkStream stream, NetPacket packet)
            => await WritePacketAsync(stream, packet.ToRawPacket());
        public static async Task WritePacketAsync(NetworkStream stream, RawNetPacket packet)
        {
            var packetStruct = StructTools.RawSerialize(packet);
            await stream.WriteAsync(packetStruct, 0, packetStruct.Length);
        }

        public static async Task<NetPacket> ReadPacketAsync(NetworkStream stream, int maxSize)
        {
            var buffer = new byte[maxSize];
            var bytesRead = await stream.ReadAsync(buffer, 0, maxSize);

            Array.Resize(ref buffer, bytesRead);

            var rawPacket = StructTools.RawDeserialize<RawNetPacket>(buffer, 0);
            return NetPacket.FromRawPacket(rawPacket);
        }
    }
}
