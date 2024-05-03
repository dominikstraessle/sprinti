using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sprinti.Confirmation;
using Sprinti.Display;
using Sprinti.Serial;
using Sprinti.Stream;
using Sprinti.Workflow;

namespace Sprinti.Controllers;

public class InfoController(
    IOptions<SerialOptions> serialOptions,
    IOptions<ConfirmationOptions> confirmationOptions,
    IOptions<StreamOptions> streamOptions,
    DetectionOptions detectionOptions,
    IOptions<DisplayOptions> displayOptions,
    IOptions<CaptureOptions> captureOptions,
    IOptions<WorkflowOptions> workflowOptions,
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
            Detection = detectionOptions,
            Environment = environment.EnvironmentName,
            Capture = captureOptions.Value,
            Display = displayOptions.Value,
            Workflow = workflowOptions.Value
        });
    }

    [HttpPost(nameof(UpdateDetection), Name = nameof(UpdateDetection))]
    [ProducesResponseType(typeof(InfoDto), 200)]
    public IActionResult UpdateDetection([FromBody] DetectionOptions newOptions)
    {
        detectionOptions.LookupConfigs = newOptions.LookupConfigs;
        return Ok(new InfoDto
        {
            Stream = streamOptions.Value,
            Confirmation = confirmationOptions.Value,
            Serial = serialOptions.Value,
            Detection = detectionOptions,
            Environment = environment.EnvironmentName,
            Capture = captureOptions.Value,
            Display = displayOptions.Value,
            Workflow = workflowOptions.Value
        });
    }

    public class InfoDto
    {
        public required SerialOptions Serial { get; init; }
        public required ConfirmationOptions Confirmation { get; init; }
        public required StreamOptions Stream { get; init; }
        public required CaptureOptions Capture { get; init; }
        public required DisplayOptions Display { get; init; }
        public required string Environment { get; set; }
        public required WorkflowOptions Workflow { get; set; }
        public required DetectionOptions Detection { get; init; }
    }
}