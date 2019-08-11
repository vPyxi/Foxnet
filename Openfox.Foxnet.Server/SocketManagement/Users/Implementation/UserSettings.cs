using System;
using System.Collections.Generic;
using System.Text;

namespace Openfox.Foxnet.Server.SocketManagement.Users.Implementation
{
    public class UserState
    {
        public bool HasHandshakedWithServer { get; set; }
        public string Username { get; set; }
    }
}
