using moosik.dal.Models;

namespace moosik.services.Dtos;

public class UserDetailDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public ThreadDto[] Threads { get; set; }
    public Post[] Posts { get; set; }
    public UserRoleDto Role { get; set; }
}