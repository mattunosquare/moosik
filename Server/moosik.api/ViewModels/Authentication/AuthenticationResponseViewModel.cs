using System.Diagnostics.CodeAnalysis;

namespace moosik.api.ViewModels.Authentication;
[ExcludeFromCodeCoverage]
public class AuthenticationResponseViewModel
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}