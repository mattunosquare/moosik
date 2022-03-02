using System.Diagnostics.CodeAnalysis;

namespace moosik.services.Dtos.Post;
[ExcludeFromCodeCoverage]
public class CreatePostDto
{
    public int ThreadId { get; set; }
    
    public int UserId { get; set; }

    public string Description { get; set; }
    
    public CreatePostResourceDto Resource { get; set; }
}