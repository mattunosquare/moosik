using System.Diagnostics.CodeAnalysis;

namespace moosik.services.Dtos;

[ExcludeFromCodeCoverage]
public class UpdateUserDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}