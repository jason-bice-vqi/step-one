using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViaQuestInc.StepOne.Core.Candidates.Services;
using ViaQuestInc.StepOne.Infrastructure.Auth;

namespace ViaQuestInc.StepOne.Web.Controllers.Candidates;

[ApiController]
[Route("candidates")]
public class CandidatesController(CandidateService candidateService) : ApiControllerBase
{
    [AllowAnonymous]
    [ApiKeyAuthorization]
    [HttpPost("import")]
    public async Task<IActionResult> ImportBulk(CancellationToken cancellationToken)
    {
        if (Request.ContentLength == 0) return BadRequest("No file uploaded.");
                
        if (!Request.ContentType?.Contains(
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            ) ??
            false)
        {
            return BadRequest("Invalid file format. Please upload an .xlsx file.");
        }

        using var memoryStream = new MemoryStream();
                
        await Request.Body.CopyToAsync(memoryStream, cancellationToken);

        await candidateService.ImportAsync(memoryStream, cancellationToken);

        return NoContent();
    }
}