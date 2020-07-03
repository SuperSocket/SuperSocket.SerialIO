using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperSocket.SerialIO;

namespace SuperSocket
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseSerialIO(this ISuperSocketHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices(
                (hostCtx, services) =>
                {
                    services.AddSingleton<IChannelCreatorFactory, SerialIOChannelCreatorFactory>();
                }
            );
        }
    }
}
