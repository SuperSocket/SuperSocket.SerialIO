using SuperSocket.Connection;
using SuperSocket.Server.Abstractions;
using SuperSocket.Server.Abstractions.Connections;

namespace SuperSocket.SerialIO
{
    internal class SerialIOConnectionFactoryBuilder : IConnectionFactoryBuilder
    {
        public IConnectionFactory Build(ListenOptions listenOptions, ConnectionOptions connectionOptions)
        {
            return new SerialIOConnectionFactory(listenOptions, connectionOptions);
        }
    }
}