using moosik.api.ViewModels.User;

namespace moosik.api.ViewModels;

public class PostViewModel
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public int ThreadId { get; set; }
    public UserViewModel User { get; set; }
    public IEnumerable<PostResourceViewModel> Resources { get; set; }
}