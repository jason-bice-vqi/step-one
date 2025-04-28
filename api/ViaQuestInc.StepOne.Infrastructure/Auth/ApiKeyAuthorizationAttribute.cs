using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ViaQuestInc.StepOne.Infrastructure.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAuthorizationAttribute : Attribute, IAuthorizationFilter
{
    private const string ApiKeyHeaderName = "API_Key";

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey))
        {
            context.Result = new UnauthorizedResult();
            
            return;
        }
        
        if (!Constants.AuthorizeApiKey(potentialApiKey.ToString()))
        {
            context.Result = new UnauthorizedResult();
        }
    }
}