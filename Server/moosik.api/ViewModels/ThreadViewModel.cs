using System.ComponentModel.DataAnnotations;

namespace moosik.api.ViewModels;

public class ThreadViewModel
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public int ThreadTypeId { get; set; }
    
    public int UserId { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public bool Active { get; set; }
}