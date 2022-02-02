using moosik.api.ViewModels.User;

namespace moosik.api.ViewModels.Thread;

public class ThreadViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedDate { get; set; }
    public ThreadTypeViewModel ThreadType { get; set; }
    public UserViewModel User { get; set; }
    public IEnumerable<PostViewModel> Posts { get; set; }
}