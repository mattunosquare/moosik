using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace moosik.dal.Models;
[ExcludeFromCodeCoverage]
[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("username")]
    public string Username { get; set; }
    
    [Column("password")]
    public string Password { get; set; }
    
    [Column("email")]
    public string Email { get; set; }
    
    [Column("active")]
    public bool Active { get; set; }
    
    [Column("role_id")]
    public int UserRoleId { get; set; }
    
    [ForeignKey(nameof(UserRoleId))]
    public UserRole Role { get; set; }

    public List<Thread> Threads { get; set; }
    
    public List<Post> Posts { get; set; }
}