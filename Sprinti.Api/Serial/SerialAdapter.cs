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

    public SerialAdapter(IOptions<SerialOptions> options, SerialPort serialPort)
    {
        _serialPort = serialPort;
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
        _serialPort.WriteLine(message);
    }

    public string ReadLine()
    {
        return _serialPort.ReadLine();
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