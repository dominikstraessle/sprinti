using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Sprinti.Api.Domain;
using Sprinti.Api.Serial;
using static Sprinti.Api.Domain.Color;
using static Sprinti.Api.Domain.Direction;
using static Sprinti.Api.Serial.ResponseState;

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
            { new RotateCommand(90), "complete", Complete },
            { new EjectCommand(Red), "complete", Complete },
            { new LiftCommand(Down), "complete", Complete },
            { new ResetCommand(), "complete", Complete },
            { new ResetCommand(), "error invalid_argument", InvalidArgument },
            { new ResetCommand(), "error not_implemented", NotImplemented },
            { new ResetCommand(), "error machine_error", MachineError },
            { new ResetCommand(), "error error", Error },
            { new ResetCommand(), "some other response", Unknown },
            { new FinishCommand(), "finish 10", Finished }
        };

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
    [MemberData(nameof(TestCommandsData))]
    public async Task TestCommands(ISerialCommand command, string response, ResponseState responseState)
    {
        _adapterMock.Setup(adapter => adapter.ReadLine()).Returns(response);

        var result = await _service.SendCommand(command, CancellationToken.None);

        Assert.Equal(responseState, result.ResponseState);
        _adapterMock.Verify(adapter => adapter.WriteLine(command.ToAsciiCommand()), Times.Once);
    }
}