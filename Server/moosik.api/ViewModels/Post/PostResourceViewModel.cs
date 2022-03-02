using System.Diagnostics.CodeAnalysis;

namespace moosik.api.ViewModels;
[ExcludeFromCodeCoverage]
public class PostResourceViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Value { get; set; }
    public ResourceTypeViewModel ResourceType { get; set; }
}