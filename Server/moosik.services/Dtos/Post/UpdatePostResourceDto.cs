using System.Diagnostics.CodeAnalysis;

namespace moosik.services.Dtos;

[ExcludeFromCodeCoverage]
public class UpdatePostResourceDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Value { get; set; }
}