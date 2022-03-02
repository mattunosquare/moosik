using System.Diagnostics.CodeAnalysis;

namespace moosik.services.Dtos;
[ExcludeFromCodeCoverage]
public class CreatePostResourceDto
{
    public string Title { get; set; }
    public string Value { get; set; }
    public int TypeId { get; set; }
}