using System.ComponentModel.DataAnnotations;

namespace moosik.api.ViewModels;

public class Post
{
    [Required]
    public int Id { get; set; }   
    
    public string Description { get; set; }
    
    public int UserId { get; set; }
    
    public int ThreadId { get; set; }
    
    public DateTime CreatedDate { get; set; }
}