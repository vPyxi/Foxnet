using Openfox.Foxnet.Server.SocketManagement.Users;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Openfox.Foxnet.Server.SocketManagement.Users.Implementation
{
    public class SocketUser : ISocketUser
    {
        public TcpClient TcpClient { get; internal set; }
        public UserState State { get; set; } = new UserState();

        public SocketUser() { }
        public SocketUser(TcpClient client)
        {
            TcpClient = client;
        }

        public NetworkStream Stream 
        {
            get {
                if (TcpClient == null)
                    throw new Exception($"{nameof(TcpClient)} is null. No connection exists for this SocketUser");
                return TcpClient.GetStream();
            }
        }

        public byte GetNextPacket()
        {
            return 0;
        }
    }
}
