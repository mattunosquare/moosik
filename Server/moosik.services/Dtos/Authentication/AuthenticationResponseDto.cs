namespace moosik.services.Dtos.Authentication;

public class AuthenticationResponseDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}