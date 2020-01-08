using System;
using System.Buffers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SuperSocket.Channel;
using SuperSocket.ProtoBase;

namespace SuperSocket.SerialIO
{
    public class SerialIOChannelCreatorFactory : IChannelCreatorFactory
    {
        public IChannelCreator CreateChannelCreator<TPackageInfo>(ListenOptions options, ChannelOptions channelOptions, ILoggerFactory loggerFactory, object pipelineFilterFactory) where TPackageInfo : class
        {
            var filterFactory = pipelineFilterFactory as IPipelineFilterFactory<TPackageInfo>;
            channelOptions.Logger = loggerFactory.CreateLogger(nameof(IChannel));
            var channelFactoryLogger = loggerFactory.CreateLogger(nameof(SerialIOChannelCreator));            
            return new SerialIOChannelCreator(options, null, channelFactoryLogger);
        }
    }
}