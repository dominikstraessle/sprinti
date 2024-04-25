using System.IO.Ports;

namespace Sprinti.Serial;

public class SerialOptions : ISprintiOptions
{
    public const string Serial = "Serial";
    public string PortName { get; set; } = "/dev/pts/0";
    public int BaudRate { get; init; } = 115200;
    public Parity Parity { get; init; } = Parity.None;
    public int DataBits { get; init; } = 8;
    public StopBits StopBits { get; init; } = StopBits.One;
    public int ReadTimeoutInMilliseconds { get; init; } = 10000;
    public int WriteTimeoutInMilliseconds { get; init; } = 5000;
    public bool Enabled { get; set; }
}