using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Sprinti.Domain;
using Sprinti.Detection;

namespace Sprinti.Controllers;

public class StreamController(
    IVideoProcessor videoProcessor
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

    public class RunDetectionDto
    {
        public required double Duration { get; set; }
        public required CubeConfig? Config { get; set; }
    }
}