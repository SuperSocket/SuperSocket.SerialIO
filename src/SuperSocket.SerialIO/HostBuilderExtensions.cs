using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperSocket.SerialIO;

namespace SuperSocket
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseSerialIO(this IHostBuilder hostBuilder)
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
