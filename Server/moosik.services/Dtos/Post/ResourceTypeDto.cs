using System.Diagnostics.CodeAnalysis;

namespace moosik.services.Dtos;

[ExcludeFromCodeCoverage]
public class ResourceTypeDto
{
    public int Id { get; set; }
    public string Description { get; set; }
}