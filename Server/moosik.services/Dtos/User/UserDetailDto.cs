namespace moosik.services.Dtos.User;

public class UserDetailDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public ThreadDto[] Threads { get; set; }
    public dal.Models.Post[] Posts { get; set; }
    public UserRoleDto Role { get; set; }
}