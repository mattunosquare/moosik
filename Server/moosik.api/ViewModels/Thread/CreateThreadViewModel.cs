using System.Diagnostics.CodeAnalysis;

namespace moosik.api.ViewModels;
[ExcludeFromCodeCoverage]
public class CreateThreadViewModel
{
    public string Title { get; set; }
    public int UserId { get; set; }
    public int ThreadTypeId { get; set; }
    public CreatePostViewModel Post { get; set; }
}