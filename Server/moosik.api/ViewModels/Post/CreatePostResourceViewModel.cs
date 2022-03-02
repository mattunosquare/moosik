using System.Diagnostics.CodeAnalysis;

namespace moosik.api.ViewModels.Post;
[ExcludeFromCodeCoverage]
public class CreatePostResourceViewModel
{
    public string Title { get; set; }
    public string Value { get; set; }
    public int TypeId { get; set; }
}