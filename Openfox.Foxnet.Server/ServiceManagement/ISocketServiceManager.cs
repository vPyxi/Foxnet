using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Openfox.Foxnet.Server.ServiceManagement
{
    public interface ISocketServiceManager
    {
        bool IsRunning { get; }
        IPAddress IPAddress { get; }
        int Port { get; }
        Exception ServerException { get; }

        Task Listen();
        Task Listen(string ip, int port);
    }
}
