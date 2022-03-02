using System.Diagnostics.CodeAnalysis;

namespace moosik.api.ViewModels.Authentication;

[ExcludeFromCodeCoverage]
public class AuthenticationRequestViewModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}