using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sprinti.Confirmation;
using Sprinti.Display;
using Sprinti.Serial;
using Sprinti.Stream;

namespace Sprinti.Controllers;

public class InfoController(
    IOptions<SerialOptions> serialOptions,
    IOptions<ConfirmationOptions> confirmationOptions,
    IOptions<StreamOptions> streamOptions,
    IOptions<DetectionOptions> imageOptions,
    IOptions<DisplayOptions> displayOptions,
    IOptions<CaptureOptions> captureOptions,
    IHostEnvironment environment
) : ApiController
{
    [HttpGet(nameof(Config), Name = nameof(Config))]
    [ProducesResponseType(typeof(InfoDto), 200)]
    public IActionResult Config()
    {
        return Ok(new InfoDto
        {
            Stream = streamOptions.Value,
            Confirmation = confirmationOptions.Value,
            Serial = serialOptions.Value,
            Detection = imageOptions.Value,
            Environment = environment.EnvironmentName,
            Capture = captureOptions.Value,
            Display = displayOptions.Value
        });
    }

    public class InfoDto
    {
        public required SerialOptions Serial { get; init; }
        public required ConfirmationOptions Confirmation { get; init; }
        public required StreamOptions Stream { get; init; }
        public required DetectionOptions Detection { get; init; }
        public required CaptureOptions Capture { get; init; }
        public required DisplayOptions Display { get; init; }
        public required string Environment { get; set; }
    }
}