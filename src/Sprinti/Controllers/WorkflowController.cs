using Microsoft.AspNetCore.Mvc;
using Sprinti.Detection;
using Sprinti.Domain;
using Sprinti.Serial;
using Sprinti.Workflow;

namespace Sprinti.Controllers;

public class WorkflowController(
    IWorkflowService workflowService,
    DetectionOptions detectionOptions,
    ISerialService serialService
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


    [HttpPost(nameof(InitWorkflow), Name = nameof(InitWorkflow))]
    [ProducesResponseType(202)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> InitWorkflow([FromBody] DetectionOptions newOptions,
        CancellationToken cancellationToken)
    {
        detectionOptions.LookupConfigs = newOptions.LookupConfigs;
        await serialService.RunStartProcedure(cancellationToken);
        return Accepted();
    }
}