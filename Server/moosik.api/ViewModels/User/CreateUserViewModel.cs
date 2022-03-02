using System.Diagnostics.CodeAnalysis;

namespace moosik.api.ViewModels.User;
[ExcludeFromCodeCoverage]
public class CreateUserViewModel
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public int RoleId { get; set; }
    
}