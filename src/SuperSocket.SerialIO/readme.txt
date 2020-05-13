This is the SerialPort function implementation for the SuperSocket framework.

Because of the particularity of serial port,One serialport only one session,so you need to differentiate the client from the protocol.

you can use the code like this to build the serial port server:

var host = SuperSocketHostBuilder
.Create<StringPackageInfo, CommandLinePipelineFilter>()
.ConfigurePackageHandler(async (s, package) =>
    {
        await s.SendAsync(Encoding.UTF8.GetBytes(result.ToString() + "\r\n"));
    })
.ConfigureLogging((hostCtx, loggingBuilder) =>{loggingBuilder.AddConsole();})

 //The important code is below
.ConfigureSuperSocket(options =>  
    {
        options.Name = "Echo Server";
        options.Listeners = new[] {
            new SerialIOListenOptions()
            {
                Ip = "COM2",   //serial port name
                BaudRate = 9600,  //baudRate of the serial port
                Parity = Parity.None,
                StopBits= StopBits.None,
                Databits=5;  //value limit 5 to 8
            }
        };
    })
.ConfigureServices(
    (hostCtx, services) =>
    {
        //at here to replace the channel factory, use the SerialIOChannelCreatorFactory.
        services.Replace(ServiceDescriptor.Singleton<IChannelCreatorFactory, SerialIOChannelCreatorFactory>());
    }
)

