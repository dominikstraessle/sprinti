using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sprinti.Confirmation;
using Sprinti.Serial;
using Sprinti.Stream;

namespace Sprinti.Controllers;

public class InfoController(
    IOptions<SerialOptions> serialOptions,
    IOptions<ConfirmationOptions> confirmationOptions,
    IOptions<StreamOptions> streamOptions,
    IOptions<ImageOptions> imageOptions,
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
            Image = imageOptions.Value,
            Environment = environment.EnvironmentName
        });
    }

    public class InfoDto
    {
        public required SerialOptions Serial { get; init; }
        public required ConfirmationOptions Confirmation { get; init; }
        public required StreamOptions Stream { get; init; }
        public required ImageOptions Image { get; init; }
        public required string Environment { get; set; }
    }
}