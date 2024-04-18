using Microsoft.AspNetCore.Mvc;
using Sprinti.Workflow;

namespace Sprinti.Controllers;

public class WorkflowController(
    IWorkflowService workflowService
) : ApiController
{
    [HttpGet(nameof(RunWorkflow), Name = nameof(RunWorkflow))]
    [ProducesResponseType(200)]
    public async Task<IActionResult> RunWorkflow([FromQuery] int timeout = 60)
    {
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(timeout));

        await workflowService.RunAsync(cancellationTokenSource.Token);
        return Ok();
    }
}