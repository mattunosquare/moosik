namespace moosik.api.ViewModels.Authentication;

public class AuthenticationResponseViewModel
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}