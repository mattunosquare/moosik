using System.Diagnostics.CodeAnalysis;

namespace moosik.api.ViewModels.User;
[ExcludeFromCodeCoverage]
public class UserViewModel
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}