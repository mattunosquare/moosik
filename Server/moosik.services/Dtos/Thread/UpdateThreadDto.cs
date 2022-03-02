using System.Diagnostics.CodeAnalysis;

namespace moosik.services.Dtos;

[ExcludeFromCodeCoverage]
public class UpdateThreadDto
{
    public int Id { get; set; }
    public string Title { get; set; }
}