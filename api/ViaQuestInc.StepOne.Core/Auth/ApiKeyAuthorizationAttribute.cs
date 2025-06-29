using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ViaQuestInc.StepOne.Core.Kernel;

namespace ViaQuestInc.StepOne.Core.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAuthorizationAttribute
    : Attribute,
        IAuthorizationFilter
{
    private const string ApiKeyHeaderName = "API_Key";

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey))
        {
            context.Result = new UnauthorizedResult();

            return;
        }

        if (!Constants.AuthorizeApiKey(potentialApiKey.ToString())) context.Result = new UnauthorizedResult();
    }
}