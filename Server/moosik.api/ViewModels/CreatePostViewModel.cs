namespace moosik.api.ViewModels;

public class CreatePostViewModel
{
    public int ThreadId { get; set; }
    
    public int UserId { get; set; }

    public string Description { get; set; }
    
    public string PostResourceTitle { get; set; }
    
    public string PostResourceValue { get; set; }
    
    public int ResourceTypeId { get; set; }
}