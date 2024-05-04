using Microsoft.Extensions.Logging;
using SuperSocket.Connection;
using SuperSocket.Server.Abstractions;
using SuperSocket.Server.Abstractions.Connections;

namespace SuperSocket.SerialIO
{
    public class SerialIOConnectionListenerFactory : IConnectionListenerFactory
    {
        protected IConnectionFactoryBuilder ConnectionFactoryBuilder { get; }

        public SerialIOConnectionListenerFactory(IConnectionFactoryBuilder connectionFactoryBuilder)
        {
            ConnectionFactoryBuilder = connectionFactoryBuilder;
        }


        public IConnectionListener CreateConnectionListener(ListenOptions options, ConnectionOptions connectionOptions, ILoggerFactory loggerFactory)
        {
            var connectionFactory = ConnectionFactoryBuilder.Build(options, connectionOptions);
            return new SerialIOConnectionListener(options, connectionFactory, loggerFactory.CreateLogger<SerialIOConnectionListener>());
        }
    }
}