using moosik.api.ViewModels.Post;

namespace moosik.api.ViewModels;

public class CreatePostViewModel
{
    public int ThreadId { get; set; }
    
    public int UserId { get; set; }

    public string Description { get; set; }

    public CreatePostResourceViewModel? Resource { get; set; }
    
}