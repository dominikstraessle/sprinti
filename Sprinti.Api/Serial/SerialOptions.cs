using System.IO.Ports;

namespace Sprinti.Api.Serial;

public class SerialOptions
{
    public const string ConfigurationSectionName = "Serial";

    public required string PortName { get; init; } = "/dev/pts/2";
    public required int BaudRate { get; init; } = 115200;
    public required Parity Parity { get; init; } = Parity.None;
    public required int DataBits { get; init; } = 8;
    public required StopBits StopBits { get; init; } = StopBits.One;
    public required int ReadTimeout { get; init; } = 500;
    public required int WriteTimeout { get; init; } = 500;
}