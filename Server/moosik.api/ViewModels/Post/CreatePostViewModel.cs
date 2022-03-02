using System.Diagnostics.CodeAnalysis;
using moosik.api.ViewModels.Post;

namespace moosik.api.ViewModels;
[ExcludeFromCodeCoverage]
public class CreatePostViewModel
{
    public int UserId { get; set; }

    public string Description { get; set; }

    public CreatePostResourceViewModel? Resource { get; set; }
    
}