using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace moosik.dal.Models;
[ExcludeFromCodeCoverage]
[Table("user_roles")]
public class UserRole
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("description")]
    public string Description { get; set; }
}