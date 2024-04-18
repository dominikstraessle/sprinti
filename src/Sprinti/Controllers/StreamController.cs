using Microsoft.AspNetCore.Mvc;
using Sprinti.Domain;
using Sprinti.Stream;

namespace Sprinti.Controllers;

public class StreamController(
    IVideoProcessor videoProcessor
) : ApiController
{
    [HttpGet(nameof(RunDetection), Name = nameof(RunDetection))]
    [ProducesResponseType(typeof(CubeConfig), 200)]
    public Task<IActionResult> RunDetection([FromQuery] int timeout = 20)
    {
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(timeout));

        var config = videoProcessor.RunDetection(cancellationTokenSource.Token);
        return Task.FromResult<IActionResult>(Ok(config));
    }
}