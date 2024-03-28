using System.IO.Ports;
using Microsoft.Extensions.Options;

namespace Sprinti.Serial;

public interface ISerialAdapter : IDisposable
{
    void WriteLine(string text);
    string ReadLine();
}

internal class SerialAdapter : SerialPort, ISerialAdapter
{
    private readonly ILogger<SerialAdapter> _logger;

    public SerialAdapter(IOptions<SerialOptions> options, ILogger<SerialAdapter> logger)
    {
        _logger = logger;
        PortName = options.Value.PortName;
        BaudRate = options.Value.BaudRate;
        Parity = options.Value.Parity;
        DataBits = options.Value.DataBits;
        StopBits = options.Value.StopBits;
        ReadTimeout = options.Value.ReadTimeoutInMilliseconds;
        WriteTimeout = options.Value.WriteTimeoutInMilliseconds;
        logger.LogInformation("Opening serial connection");
        Open();
    }

    public new void WriteLine(string text)
    {
        _logger.LogInformation("Send serial message: '{text}'", text);
        base.WriteLine(text);
    }

    public new string ReadLine()
    {
        var responseLine = base.ReadLine();
        _logger.LogInformation("Received serial response: '{responseLine}'", responseLine);
        return responseLine;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _logger.LogInformation("Closing serial connection");

        base.Dispose(disposing);
    }
}