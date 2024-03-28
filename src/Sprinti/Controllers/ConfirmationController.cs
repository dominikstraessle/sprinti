using Microsoft.AspNetCore.Mvc;
using Sprinti.Confirmation;

namespace Sprinti.Controllers;

public class ConfirmationController(IConfirmationAdapter confirmationAdapter) : ApiController
{
    [HttpPost(nameof(Health), Name = nameof(Health))]
    [ProducesResponseType(typeof(bool), 202)]
    public async Task<IActionResult> Health(CancellationToken cancellationToken)
    {
        var isHealthy = await confirmationAdapter.HealthCheckAsync(cancellationToken);
        return Accepted(isHealthy);
    }

    [HttpPost(nameof(StartPren), Name = nameof(StartPren))]
    [ProducesResponseType(202)]
    public async Task<IActionResult> StartPren(CancellationToken cancellationToken)
    {
        await confirmationAdapter.StartAsync(cancellationToken);
        return Accepted();
    }

    [HttpPost(nameof(End), Name = nameof(End))]
    [ProducesResponseType(202)]
    public async Task<IActionResult> End(CancellationToken cancellationToken)
    {
        await confirmationAdapter.EndAsync(cancellationToken);
        return Accepted();
    }
}