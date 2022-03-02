using System.Diagnostics.CodeAnalysis;
using moosik.services.Dtos.Post;
using moosik.services.Dtos.Thread;

namespace moosik.services.Dtos.User;
[ExcludeFromCodeCoverage]
public class UserDetailDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public ThreadDto[] Threads { get; set; }
    public PostDto[] Posts { get; set; }
    public UserRoleDto Role { get; set; }
}