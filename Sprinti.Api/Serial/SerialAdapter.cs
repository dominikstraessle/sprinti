using System.IO.Ports;
using Microsoft.Extensions.Options;

namespace Sprinti.Api.Serial;

public interface ISerialAdapter : IDisposable
{
    void WriteLine(string message);
    string ReadLine();
}

public class SerialAdapter : ISerialAdapter
{
    private readonly SerialPort _serialPort;
    private readonly ILogger<SerialAdapter> _logger;

    public SerialAdapter(IOptions<SerialOptions> options, SerialPort serialPort, ILogger<SerialAdapter> logger)
    {
        _serialPort = serialPort;
        _logger = logger;
        var serialOptions = options.Value;
        serialPort.PortName = serialOptions.PortName;
        serialPort.BaudRate = serialOptions.BaudRate;
        serialPort.Parity = serialOptions.Parity;
        serialPort.DataBits = serialOptions.DataBits;
        serialPort.StopBits = serialOptions.StopBits;


        // Set the read/write timeouts
        _serialPort.ReadTimeout = serialOptions.ReadTimeout;
        _serialPort.WriteTimeout = serialOptions.WriteTimeout;

        _serialPort.Open();
    }


    public void WriteLine(string message)
    {
        _logger.LogInformation("Send serial message: '{message}'", message);
        _serialPort.WriteLine(message);
    }

    public string ReadLine()
    {
        var responseLine = _serialPort.ReadLine();
        _logger.LogInformation("Received serial response: '{responseLine}'", responseLine);
        return responseLine;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        _serialPort.Close();
        _serialPort.Dispose();
    }
}