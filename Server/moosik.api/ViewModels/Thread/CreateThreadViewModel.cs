namespace moosik.api.ViewModels;

public class CreateThreadViewModel
{
    public string Title { get; set; }
    public int UserId { get; set; }
    public int ThreadTypeId { get; set; }
    public CreatePostViewModel Post { get; set; }
}