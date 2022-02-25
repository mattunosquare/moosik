using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace moosik.api.integration.test.ContainerManager.Auth;
public class TestAuthHandler : AuthenticationHandler<TestAuthHandlerOptions>
{

    public TestAuthHandler(IOptionsMonitor<TestAuthHandlerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
        
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        //Default Claim
        var claims = new List<Claim>{(new Claim(ClaimTypes.Name, "Test user"))};
        
        //Additional Claims
        claims.Add(new Claim(ClaimTypes.NameIdentifier, Options.FakeUserId));
        claims.Add(new Claim(ClaimTypes.Role, Options.FakeUserRole));
        
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }


}
