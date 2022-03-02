using System.Diagnostics.CodeAnalysis;

namespace moosik.api.ViewModels.User;
[ExcludeFromCodeCoverage]
public class LoggedInUserViewModel
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
}