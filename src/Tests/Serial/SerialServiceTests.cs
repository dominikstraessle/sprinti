using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Sprinti.Domain;
using Sprinti.Serial;
using static Sprinti.Domain.Color;
using static Sprinti.Domain.Direction;
using static Sprinti.Serial.ResponseState;

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

    public static TheoryData3<ISerialCommand, string, ResponseState> TestCommandsData =>
        new()
        {
            { new RotateCommand(90), "error 0", Complete },
            { new EjectCommand(Red), "error 0", Complete },
            { new LiftCommand(Down), "error 0", Complete },
            { new StartCommand(), "error 0", Complete },
            { new StartCommand(), "error 1", NotImplemented },
            { new StartCommand(), "error 2", Error },
            { new StartCommand(), "some other response", Unknown },
            { new FinishCommand(), "finish 10", Finished }
        };

    [Fact]
    public async Task TestSendCommandWithParams()
    {
        _adapterMock.Setup(adapter => adapter.ReadLine()).Returns("error 0");
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

    [Fact]
    public async Task TestSendRawCommand()
    {
        const int powerInWattHours = 10;
        var expectedResult = $"finish {powerInWattHours}";
        _adapterMock.Setup(adapter => adapter.ReadLine()).Returns(expectedResult);
        var command = new FinishCommand();

        var result = await _service.SendRawCommand(command.ToAsciiCommand(), CancellationToken.None);

        Assert.Equal(expectedResult, result);
        _adapterMock.Verify(adapter => adapter.WriteLine(command.ToAsciiCommand()), Times.Once);
    }

    [Fact]
    public async Task TestRunInstructionsAndFinish()
    {
        var expectedSequence = new List<ISerialCommand>
        {
            new StartCommand(),
            new RotateCommand(90),
            new EjectCommand(Red),
            new LiftCommand(Down),
            new FinishCommand()
        };
        const int powerInWattHours = 10;
        foreach (var serialCommand in expectedSequence)
        {
            if (serialCommand is FinishCommand)
            {
                _adapterMock.Setup(adapter => adapter.ReadLine()).Returns($"finish {powerInWattHours}");
                continue;
            }

            _adapterMock.Setup(adapter => adapter.ReadLine()).Returns("error 0");
        }

        var resultPower = await _service.RunInstructionsAndFinish([
            new RotateCommand(90),
            new EjectCommand(Red)
        ], CancellationToken.None);

        Assert.Equal(powerInWattHours, resultPower);
        foreach (var command in expectedSequence)
            _adapterMock.Verify(adapter => adapter.WriteLine(command.ToAsciiCommand()), Times.Once);
    }

    [Theory]
    [MemberData(nameof(TestCommandsData))]
    public async Task TestCommands(ISerialCommand command, string response, ResponseState responseState)
    {
        _adapterMock.Setup(adapter => adapter.ReadLine()).Returns(response);

        var result = await _service.SendCommand(command, CancellationToken.None);

        Assert.Equal(responseState, result.ResponseState);
        _adapterMock.Verify(adapter => adapter.WriteLine(command.ToAsciiCommand()), Times.Once);
    }
}