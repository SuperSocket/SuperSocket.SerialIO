using System;
using System.Buffers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SuperSocket.Channel;

namespace SuperSocket.SerialIO
{
    public class SerialIOChannelCreator : IChannelCreator
    {
        private ILogger _logger;

        private Func<object, Task<IChannel>> _channelFactory;
        
        public ListenOptions Options { get;  }

        public bool IsRunning { get; private set; }

        public event NewClientAcceptHandler NewClientAccepted;

        public SerialIOChannelCreator(ListenOptions options, Func<object, Task<IChannel>> channelFactory, ILogger logger)
        {
            Options = options;
            _channelFactory = channelFactory;
            _logger = logger;
        }

        public Task<IChannel> CreateChannel(object connection)
        {
            throw new NotImplementedException();
        }

        public bool Start()
        {
            throw new NotImplementedException();
        }

        public Task StopAsync()
        {
            throw new NotImplementedException();
        }
    }
}