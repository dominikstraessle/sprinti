using System.IO.Ports;

namespace Sprinti.Api.Serial;

public class SerialOptions
{
    public const string Serial = "Serial";
    public bool Enabled { get; set; }
    public string PortName { get; set; } = "/dev/pts/1";
    public int BaudRate { get; init; } = 115200;
    public Parity Parity { get; init; } = Parity.None;
    public int DataBits { get; init; } = 8;
    public StopBits StopBits { get; init; } = StopBits.One;
    public int ReadTimeoutInMilliseconds { get; init; } = 10000;
    public int WriteTimeoutInMilliseconds { get; init; } = 5000;
}