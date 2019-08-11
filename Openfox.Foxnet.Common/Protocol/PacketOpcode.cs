using System;
using System.Collections.Generic;
using System.Text;

namespace Openfox.Foxnet.Common.Protocol
{
    public enum PacketOpcode
    {
        AnnounceAlive,
        AnnounceAlive_Response,

        AnnounceLeaving, // Meant to be a fire & forget notification to the server.

        AuthRequest,
        AuthRequest_Response,

        GetRoomList,
        GetRoomList_Response,

        GetProtocolVersion,
        GetProtocolVersion_Response,

        Ping,
        Ping_Response,

        PingTime,
        PingTime_Response,
    }
}
