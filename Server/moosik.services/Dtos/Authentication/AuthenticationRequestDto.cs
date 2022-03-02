using System.Diagnostics.CodeAnalysis;

namespace moosik.services.Dtos.Authentication;

[ExcludeFromCodeCoverage]
public class AuthenticationRequestDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}