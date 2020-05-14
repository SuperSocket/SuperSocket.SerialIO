using System;
using Xunit;
using System.IO.Ports;
using SuperSocket.SerialIO;

namespace Test
{
    public class MainTest
    {
        [Fact]
        public void TestSerialIOEndPoint()
        {
            var sioEndPoint = new SerialIOEndPoint("sio://COM1/?BaudRate=9600&Parity=Odd&StopBits=2&Databits=7");
            
            Assert.Equal("COM1", sioEndPoint.PortName, StringComparer.OrdinalIgnoreCase);
            Assert.Equal(9600, sioEndPoint.BaudRate);      
            Assert.Equal(Parity.Odd, sioEndPoint.Parity);
            Assert.Equal(StopBits.Two, sioEndPoint.StopBits);
            Assert.Equal(7, sioEndPoint.Databits);
        }
    }
}
