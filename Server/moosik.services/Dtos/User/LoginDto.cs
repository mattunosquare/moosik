using System.Diagnostics.CodeAnalysis;

namespace moosik.services.Dtos;

[ExcludeFromCodeCoverage]
public class LoginDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}