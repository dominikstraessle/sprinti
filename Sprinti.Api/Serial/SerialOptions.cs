using System.IO.Ports;

namespace Sprinti.Api.Serial;

public class SerialOptions
{
    public const string ConfigurationSectionName = "Serial";

    public required string PortName { get; init; } = "/dev/pts/1";
    public required int BaudRate { get; init; } = 115200;
    public required Parity Parity { get; init; } = Parity.None;
    public required int DataBits { get; init; } = 8;
    public required StopBits StopBits { get; init; } = StopBits.One;
    public required int ReadTimeoutInMilliseconds { get; init; } = 10000;
    public required int WriteTimeout { get; init; } = 5000;
}