using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Openfox.Foxnet.Common.Protocol
{
    [StructLayout(LayoutKind.Explicit)]
    public struct RawNetPacket
    {
        [FieldOffset(0)]
        public byte Opcode;
        [FieldOffset(1)]
        public byte PayloadType;
        [FieldOffset(2)]
        public int PayloadSize;
    }
}
