namespace moosik.api.ViewModels;

public class CreateThreadViewModel
{
    public string Title { get; set; }
    
    public string PostDescription { get; set; }
    
    public int ThreadTypeId { get; set; }
    
    public int UserId { get; set; }
    
    public string PostResourceTitle { get; set; }
    
    public string PostResourceValue { get; set; }
    
    public int ResourceTypeId { get; set; }
}