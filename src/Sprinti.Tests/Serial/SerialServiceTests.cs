using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Sprinti.Serial;
using static Sprinti.Serial.EnumMapper;
using static Sprinti.Serial.EnumMapper.Color;
using static Sprinti.Serial.EnumMapper.ResponseState;

namespace Sprinti.Tests.Serial;

public class SerialServiceTests
{
    private readonly Mock<ISerialAdapter> _adapterMock;
    private readonly SerialService _service;

    public SerialServiceTests()
    {
        _adapterMock = new Mock<ISerialAdapter>();
        var options = new OptionsWrapper<SerialOptions>(new SerialOptions());
        _service = new SerialService(_adapterMock.Object, options, NullLogger<SerialService>.Instance);
    }

    [Fact]
    public async Task TestSendCommandWithParams()
    {
        _adapterMock.Setup(adapter => adapter.ReadLine()).Returns("complete");
        var command = new EjectCommand(Blue);

        var result = await _service.SendCommand(command, CancellationToken.None);

        Assert.Equal(Complete, result.ResponseState);
        _adapterMock.Verify(adapter => adapter.WriteLine(command.ToAsciiCommand()), Times.Once);
    }

    [Fact]
    public async Task TestSendFinish()
    {
        const int powerInWattHours = 10;
        _adapterMock.Setup(adapter => adapter.ReadLine()).Returns($"finish {powerInWattHours}");
        var command = new FinishCommand();

        var result = await _service.SendCommand(command, CancellationToken.None);

        Assert.Equal(Finished, result.ResponseState);
        Assert.Equal(powerInWattHours, result.PowerInWattHours);
        _adapterMock.Verify(adapter => adapter.WriteLine(command.ToAsciiCommand()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(CommandsTheoryData))]
    public async Task TestCommands(ISerialCommand command, string response, ResponseState responseState)
    {
        _adapterMock.Setup(adapter => adapter.ReadLine()).Returns(response);

        var result = await _service.SendCommand(command, CancellationToken.None);

        Assert.Equal(responseState, result.ResponseState);
        _adapterMock.Verify(adapter => adapter.WriteLine(command.ToAsciiCommand()), Times.Once);
    }
}