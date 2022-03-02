using System.Diagnostics.CodeAnalysis;
using moosik.api.ViewModels.Thread;

namespace moosik.api.ViewModels.User;
[ExcludeFromCodeCoverage]
public class UserDetailViewModel
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public IEnumerable<ThreadViewModel> Threads { get; set; }
    public IEnumerable<PostViewModel> Posts { get; set; }
    public UserRoleViewModel Role { get; set; }
}