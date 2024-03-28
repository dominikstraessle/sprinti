using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sprinti.Confirmation;
using Sprinti.Serial;
using Sprinti.Stream;

namespace Sprinti.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InfoController(
    IOptions<SerialOptions> serialOptions,
    IOptions<ConfirmationOptions> confirmationOptions,
    IOptions<StreamOptions> streamOptions
) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(InfoDto), 200)]
    public IActionResult Get()
    {
        return Ok(new InfoDto
        {
            Stream = streamOptions.Value,
            Confirmation = confirmationOptions.Value,
            Serial = serialOptions.Value
        });
    }

    public class InfoDto
    {
        public required SerialOptions Serial { get; init; }
        public required ConfirmationOptions Confirmation { get; init; }
        public required StreamOptions Stream { get; init; }
    }
}