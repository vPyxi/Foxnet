using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Openfox.Foxnet.Common.Serialization
{
    public static class StructTools
    {
        public static T RawDeserialize<T>(byte[] rawData, int position)
        {
            int rawSize = Marshal.SizeOf(typeof(T));
            if (rawSize > rawData.Length - position)
                throw new ArgumentException($"Not enough data to fill struct. Array length from position: {rawData.Length - position}, Struct length: {rawSize}");
            IntPtr buffer = Marshal.AllocHGlobal(rawSize);
            Marshal.Copy(rawData, position, buffer, rawSize);
            T deserializedValue = (T)Marshal.PtrToStructure(buffer, typeof(T));
            Marshal.FreeHGlobal(buffer);
            return deserializedValue;
        }

        public static byte[] RawSerialize(object obj)
        {
            int rawSize = Marshal.SizeOf(obj);
            IntPtr buffer = Marshal.AllocHGlobal(rawSize);
            Marshal.StructureToPtr(obj, buffer, false);
            byte[] rawData = new byte[rawSize];
            Marshal.Copy(buffer, rawData, 0, rawSize);
            Marshal.FreeHGlobal(buffer);
            return rawData;
        }
    }
}
