using Microsoft.AspNetCore.Authorization;
using System;

namespace moosik.api.Authorization.TokenAuthorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class TokenAuthorizationAttribute : AuthorizeAttribute
{
    public TokenAuthorizationAttribute(TokenTypes tokenTypes)
    {
        Policy = tokenTypes.ToString();
    }

}