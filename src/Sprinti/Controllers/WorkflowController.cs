using Microsoft.AspNetCore.Mvc;
using Sprinti.Domain;
using Sprinti.Serial;
using Sprinti.Detection;
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
    [ProducesResponseType(typeof(CompletedResponse), 202)]
    [ProducesResponseType(typeof(CompletedResponse), 400)]
    public async Task<IActionResult> InitWorkflow([FromBody] DetectionOptions newOptions,
        CancellationToken cancellationToken)
    {
        detectionOptions.LookupConfigs = newOptions.LookupConfigs;
        var response = await serialService.SendCommand(new InitCommand(), cancellationToken);
        if (response.ResponseState is not ResponseState.Complete)
        {
            return BadRequest(response);
        }

        response = await serialService.SendCommand(new AlignCommand(), cancellationToken);
        return Accepted(response);
    }
}