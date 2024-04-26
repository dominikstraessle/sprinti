using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Sprinti.Domain;
using Sprinti.Instruction;
using Sprinti.Serial;
using Sprinti.Stream;

namespace Sprinti.Controllers;

public class StreamController(
    IVideoProcessor videoProcessor,
    ISerialService serialService,
    IInstructionService instructionService
) : ApiController
{
    [HttpGet(nameof(RunDetection), Name = nameof(RunDetection))]
    [ProducesResponseType(typeof(RunDetectionDto), 200)]
    public Task<IActionResult> RunDetection([FromQuery] int timeout = 20)
    {
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(timeout));
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        var config = videoProcessor.RunDetection(cancellationTokenSource.Token);
        stopWatch.Stop();
        return Task.FromResult<IActionResult>(Ok(new RunDetectionDto
        {
            Duration = stopWatch.Elapsed.TotalSeconds,
            Config = config
        }));
    }

    [HttpGet(nameof(RunAndInstruct), Name = nameof(RunAndInstruct))]
    [ProducesResponseType(typeof(RunDetectionDto), 200)]
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

        return Ok(new RunDetectionDto
        {
            Duration = stopWatch.Elapsed.TotalSeconds,
            Config = config
        });
    }

    public class RunDetectionDto
    {
        public required double Duration { get; set; }
        public required CubeConfig? Config { get; set; }
    }
}