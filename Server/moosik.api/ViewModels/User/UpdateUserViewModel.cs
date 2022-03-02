using System.Diagnostics.CodeAnalysis;

namespace moosik.api.ViewModels.User;
[ExcludeFromCodeCoverage]
public class UpdateUserViewModel
{
    public string Username { get; set; }
    public string Email { get; set; }
}