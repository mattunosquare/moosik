using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace moosik.dal.Models;
[ExcludeFromCodeCoverage]
[Table("post_resources")]
public class PostResource
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("title")]
    public string Title { get; set; }
    
    [Column("value")]
    public string Value { get; set; }
    
    [Column("post_id")]
    public int PostId { get; set; }

    [ForeignKey(nameof(PostId))] 
    public Post Post { get; set; }
    
    [Column("resource_type_id")]
    public int ResourceTypeId { get; set; }
    
    [ForeignKey(nameof(ResourceTypeId))]
    public ResourceType ResourceType { get; set; }
}