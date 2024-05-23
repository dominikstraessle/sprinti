using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Sprinti.Detection;
using Sprinti.Instruction;
using Sprinti.Serial;

namespace Sprinti.Controllers;

public class StreamInstructController(
    IVideoProcessor videoProcessor,
    ISerialService serialService,
    IInstructionService instructionService
) : ApiController
{
    [HttpGet(nameof(RunAndInstruct), Name = nameof(RunAndInstruct))]
    [ProducesResponseType(typeof(StreamController.RunDetectionDto), 200)]
    public async Task<IActionResult> RunAndInstruct([FromQuery] int timeout = 20)
    {
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(timeout));
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        var config = videoProcessor.RunDetection(cancellationTokenSource.Token);
        stopWatch.Stop();
        if (config != null)
        {
            var instructionSequence = instructionService.GetInstructionSequence(config.Config);

            await serialService.RunInstructionsAndFinish(instructionSequence, cancellationTokenSource.Token);
        }

        return Ok(new StreamController.RunDetectionDto
        {
            Duration = stopWatch.Elapsed.TotalSeconds,
            Config = config
        });
    }
}