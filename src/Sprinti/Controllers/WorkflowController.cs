using Microsoft.AspNetCore.Mvc;
using Sprinti.Workflow;

namespace Sprinti.Controllers;

public class WorkflowController(
    ServiceProvider provider,
    WorkflowWorker worker
) : ApiController
{
    [HttpPost(nameof(RegisterWorker), Name = nameof(RegisterWorker))]
    [ProducesResponseType(202)]
    public async Task<IActionResult> RegisterWorker([FromQuery] bool liftDown,
        CancellationToken cancellationToken)
    {
        return Accepted();
    }
}