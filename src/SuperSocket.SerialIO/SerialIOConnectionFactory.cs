using System;
using System.IO.Ports;
using System.Threading.Tasks;
using SuperSocket.Connection;
using System.Threading;
using SuperSocket.Server.Abstractions;

namespace SuperSocket.SerialIO
{
    /// <summary>
    /// the serialport channel creator
    /// </summary>
    internal class SerialIOConnectionFactory : IConnectionFactory
    {
        public ListenOptions Options { get; }

        private ConnectionOptions _connectionOptions { get; }

        public SerialIOConnectionFactory(ListenOptions options, ConnectionOptions connectionOptions)
        {
            Options = options;
            _connectionOptions = connectionOptions;
        }

        public Task<IConnection> CreateConnection(object connection, CancellationToken cancellationToken)
        {
            var serialPort = connection as SerialPort;

            if (serialPort == null)
                throw new ArgumentException("connection must be SerialPort", nameof(connection));

            return Task.FromResult<IConnection>(new SerialIOPipeConnection(serialPort, _connectionOptions));
        }
    }
}