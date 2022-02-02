using Microsoft.AspNetCore.Authorization;

namespace moosik.api.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class TokenAuthorizationAttribute : AuthorizeAttribute
{
    public TokenAuthorizationAttribute(TokenTypes tokenTypes)
    {
        Policy = tokenTypes.ToString();
        
    }

}