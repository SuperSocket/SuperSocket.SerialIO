using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperSocket.SerialIO;
using SuperSocket.Server.Abstractions.Connections;
using SuperSocket.Server.Abstractions.Host;

namespace SuperSocket
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseSerialIO(this ISuperSocketHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices(
                (hostCtx, services) =>
                {
                    services.AddSingleton<IConnectionFactoryBuilder, SerialIOConnectionFactoryBuilder>();
                    services.AddSingleton<IConnectionListenerFactory, SerialIOConnectionListenerFactory>();
                }
            );
        }
    }
}
