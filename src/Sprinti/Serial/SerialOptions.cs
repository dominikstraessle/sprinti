using System.IO.Ports;

namespace Sprinti.Serial;

public class SerialOptions : ISprintiOptions
{
    public const string Serial = "Serial";
    public string PortName { get; set; } = "/dev/pts/0";
    public int BaudRate { get; set; } = 115200;
    public Parity Parity { get; set; } = Parity.None;
    public int DataBits { get; set; } = 8;
    public StopBits StopBits { get; set; } = StopBits.One;
    public int ReadTimeoutInMilliseconds { get; set; } = 10000;
    public int WriteTimeoutInMilliseconds { get; set; } = 5000;
    public int CommandDelayInMilliseconds { get; set; } = 50;
    public bool Enabled { get; set; } = true;
}