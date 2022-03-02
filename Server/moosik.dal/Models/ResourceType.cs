using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace moosik.dal.Models;
[ExcludeFromCodeCoverage]
[Table("resource_types")]
public class ResourceType
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("description")]
    public string Description { get; set; }

    private List<PostResource> PostResources { get; set; }
}