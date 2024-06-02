using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Sprinti.Button;
using Sprinti.Confirmation;
using Sprinti.Detection;
using Sprinti.Display;
using Sprinti.Instruction;
using Sprinti.Serial;
using Sprinti.Workflow;

namespace Sprinti.Tests.Workflow;

public class WorkflowTests
{
    private readonly Mock<IButtonService> _buttonService;
    private readonly Mock<IVideoProcessor> _videoService;
    private readonly Mock<ISerialService> _serialService;
    private readonly Mock<IInstructionService> _instructionService;
    private readonly Mock<IConfirmationService> _confirmationService;
    private readonly Mock<IDisplayService> _displayService;
    private readonly WorkflowService _service;

    public WorkflowTests()
    {
        _buttonService = new Mock<IButtonService>();
        _videoService = new Mock<IVideoProcessor>();
        _serialService = new Mock<ISerialService>();
        _instructionService = new Mock<IInstructionService>();
        _confirmationService = new Mock<IConfirmationService>();
        _displayService = new Mock<IDisplayService>();
        _service = new WorkflowService(
            _buttonService.Object,
            _videoService.Object,
            _serialService.Object,
            _instructionService.Object,
            _confirmationService.Object,
            _displayService.Object,
            new NullLogger<WorkflowService>()
        );
    }


    [Fact]
    public async Task ShouldStartAsync()
    {
        _displayService.Setup(service => service.Print(It.IsAny<string>()));
        _buttonService.Setup(service => service.WaitForSignalAsync(It.IsAny<CancellationToken>()));
        _serialService.Setup(service => service.RunStartProcedure(It.IsAny<CancellationToken>()));

        await _service.StartAsync(CancellationToken.None);

        _displayService.Verify(service => service.Print("Heppo ist bereit für Init"), Times.Once);
        _buttonService.Verify(service => service.WaitForSignalAsync(CancellationToken.None), Times.Once);
        _displayService.Verify(service => service.Print("Start-Prozedur wird gestartet"), Times.Once);
        _serialService.Verify(service => service.RunStartProcedure(CancellationToken.None), Times.Once);
        _displayService.Verify(service => service.Print("Start-Prozedur fertig"), Times.Once);
    }
}