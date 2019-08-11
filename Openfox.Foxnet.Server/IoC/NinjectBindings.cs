using MediatR;
using Ninject.Modules;
using Openfox.Foxnet.Server.ServiceManagement;
using Openfox.Foxnet.Server.ServiceManagement.Implementation;
using Openfox.Foxnet.Server.SocketManagement.Authentication;
using Openfox.Foxnet.Server.SocketManagement.Authentication.Implementation;
using Openfox.Foxnet.Server.SocketManagement.Rooms;
using Openfox.Foxnet.Server.SocketManagement.Users;
using Openfox.Foxnet.Server.SocketManagement.Users.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Openfox.Foxnet.Server.IoC
{
    public class NinjectBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<ISocketServiceManager>().To<SocketServiceManager>();
            Bind<IUserAuthHandler>().To<UserAuthHandler>();
            Bind<IRoomManager>().To<RoomManager>();
            Bind<ISocketUser>().To<SocketUser>();
            Bind<IMediator>().To<Mediator>();
        }
    }
}
