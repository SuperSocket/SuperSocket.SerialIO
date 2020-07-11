# SuperSocket.SerialIO

[![Join the chat at https://gitter.im/supersocket/community](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/supersocket/community)
[![Build Status](https://travis-ci.org/supersocket/SuperSocket.SerialIO.svg?branch=master)](https://travis-ci.org/supersocket/SuperSocket.SerialIO)
[![NuGet Version](https://img.shields.io/nuget/vpre/SuperSocket.SerialIO.svg?style=flat)](https://www.nuget.org/packages/SuperSocket.SerialIO/)
[![NuGet Download](https://img.shields.io/nuget/dt/SuperSocket.SerialIO.svg?style=flat)](https://www.nuget.org/packages/SuperSocket.SerialIO/)


Serial IO support for SuperSocket

The sample code below shows us how we build a serial IO communication server with SuperSocket.

```csharp
var host = SuperSocketHostBuilder
    .Create<StringPackageInfo, CommandLinePipelineFilter>()
    .UsePackageHandler(async (s, package) =>
        {
            await s.SendAsync(Encoding.UTF8.GetBytes(package.ToString() + "\r\n"));
        })
    .ConfigureSuperSocket(options =>  
        {
            options.Name = "SIOServer";
            options.Listeners = new[] {
                new SerialIOListenOptions()
                {
                    PortName = "COM2",   // serial port name
                    BaudRate = 9600,  // baudRate of the serial port
                    Parity = Parity.None,
                    StopBits = StopBits.None,
                    Databits = 5;  // value limit 5 to 8
                }
            };
        })
    .UseSerialIO()
    .ConfigureLogging((hostCtx, loggingBuilder) => {
        loggingBuilder.AddConsole();
    }).Build();
```


You also can leave the infromation of Serial IO in the configuration.

```json
{
    "serverOptions": {
        "name": "SIOServer",
        "listeners": [
            {
                "path": "sio://COM1/?BaudRate=9600&Parity=Odd&StopBits=2&Databits=7"
            }
        ]
    }
}
```