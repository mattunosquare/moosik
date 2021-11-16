using System.ComponentModel.DataAnnotations;

namespace moosik.api.ViewModels;

public class User
{
    [Required]
    public int Id { get; set; }
    
    public string Username { get; set; }
    
    public string Email { get; set; }
}