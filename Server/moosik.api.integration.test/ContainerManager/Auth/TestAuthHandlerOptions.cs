using Microsoft.AspNetCore.Authentication;

namespace moosik.api.integration.test.ContainerManager.Auth;

public class TestAuthHandlerOptions : AuthenticationSchemeOptions
{
    public string FakeUserId { get; set; }
    public string FakeUserRole { get; set; }
}