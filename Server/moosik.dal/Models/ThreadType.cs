using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace moosik.dal.Models;

[Table("thread_types")]
public class ThreadType
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("description")]
    public string Description { get; set; }
    
    public List<Thread> Threads { get; set; }
}