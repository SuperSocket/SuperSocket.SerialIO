using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SuperSocket.Connection;
using SuperSocket.Server.Abstractions;
using SuperSocket.Server.Abstractions.Connections;

namespace SuperSocket.SerialIO
{
    internal class SerialIOConnectionListener : IConnectionListener
    {
        public ListenOptions Options { get; }

        public bool IsRunning { get; private set; }

        public IConnectionFactory ConnectionFactory { get; }

        private readonly ILogger _logger;

        private SerialPort _port = null;

        public event NewConnectionAcceptHandler NewConnectionAccept;

        public SerialIOConnectionListener(ListenOptions options, IConnectionFactory connectionFactory, ILogger logger)
        {
            Options = options;
            ConnectionFactory = connectionFactory;
            _logger = logger;
            _port = CreateSerialPort(Options);
        }

        private SerialPort CreateSerialPort(ListenOptions options)
        {
            var sioEndpoint = options as ISerialIOEndPoint;

            if (sioEndpoint == null)
            {
                if (string.IsNullOrEmpty(options.Path))
                    throw new Exception("Invalid Path value in the ListenOptions.");

                sioEndpoint = new SerialIOEndPoint(options.Path);
            }

            return new SerialPort(sioEndpoint.PortName, sioEndpoint.BaudRate, sioEndpoint.Parity, sioEndpoint.Databits)
            {
                StopBits = sioEndpoint.StopBits
            };
        }

        public bool Start()
        {
            try
            {
                _port.Open();

                // Serial port is different with the tcp. It doesn't allow concurrent multiple connections.
                // We should create the single SerialIO connection at the beginning.
                var connection = ConnectionFactory.CreateConnection(_port, CancellationToken.None).Result;
                NewConnectionAccept?.Invoke(this.Options, connection);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"serial port: {_port.PortName} open fail!");
                return false;
            }
        }

        public Task StopAsync()
        {
            _port?.Close();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            var port = _port;

            if (port == null)
                return;

            if (Interlocked.CompareExchange(ref _port, null, port) == port)
            {
                port.Dispose();
            }
        }
    }
}