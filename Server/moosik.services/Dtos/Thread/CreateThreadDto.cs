using System.Diagnostics.CodeAnalysis;
using moosik.services.Dtos.Post;

namespace moosik.services.Dtos.Thread;
[ExcludeFromCodeCoverage]

public class CreateThreadDto
{
    public string Title { get; set; }
    public int UserId { get; set; }
    public int ThreadTypeId { get; set; }
    public CreatePostDto Post { get; set; }
}