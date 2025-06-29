using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ViaQuestInc.StepOne.Web.Controllers;

public class ApiControllerBase : ControllerBase
{
    protected NotFoundObjectResult CandidateNotFound(int candidateId)
    {
        return NotFound($"Candidate '{candidateId}' was not found.");
    }

    protected NotFoundObjectResult CandidateWorkflowNotFound(int candidateId)
    {
        return NotFound($"Workflow not found for candidate '{candidateId}'.");
    }

    /// <summary>
    /// An override of <see cref="ControllerBase.Forbid()"/> that formulates a 403 Forbidden response with a custom
    /// message indicating why authorization for the requested resource failed. Only the first authorization failure
    /// reason is supplied to the response.
    /// </summary>
    /// <param name="authorizationResult">The authorization result.</param>
    /// <returns>A 403 status code indicating authorization failure, along with the reason (if available).</returns>
    [NonAction]
    protected static ObjectResult Forbid(AuthorizationResult authorizationResult)
    {
        var failureMessage = string.Join(',', authorizationResult.Failure!.FailureReasons);

        return new(failureMessage)
        {
            StatusCode = (int)HttpStatusCode.Forbidden
        };
    }
}