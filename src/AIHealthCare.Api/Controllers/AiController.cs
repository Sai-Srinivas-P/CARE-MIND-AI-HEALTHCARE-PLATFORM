namespace AIHealthCare.Api.Controllers;
using AIHealthCare.Api.Contracts;
using AIHealthCare.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/ai")]
[Authorize]
public sealed class AiController(IHealthAiAssistant assistant) : ControllerBase
{
    [HttpPost("ask")]
    public async Task<ActionResult<AiAnswerResponse>> Ask(
        AiQuestionRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
            return BadRequest(new { message = "Question is required." });

        var answer = await assistant.AskAsync(request.Question, User.GetUserId(), cancellationToken);
        return Ok(new AiAnswerResponse(answer));
    }

    [HttpPost("patients/{patientId:int}/summary")]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<ActionResult<PatientSummaryResponse>> PatientSummary(
        int patientId,
        CancellationToken cancellationToken)
    {
        var summary = await assistant.BuildPatientSummaryAsync(
            patientId, User.GetUserId(), cancellationToken);

        return Ok(new PatientSummaryResponse(patientId, summary));
    }
}
