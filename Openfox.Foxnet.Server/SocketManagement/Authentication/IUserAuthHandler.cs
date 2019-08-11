using Openfox.Foxnet.Server.SocketManagement.Users.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Openfox.Foxnet.Server.SocketManagement.Authentication
{
    public interface IUserAuthHandler
    {
        void HandleAuth(SocketUser socketUser);
    }
}
