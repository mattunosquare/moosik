using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace moosik.dal.Models;

[Table("threads")]
public class Thread
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("title")]
    public string Title { get; set; }
    
    [Column("created_date")]
    public DateTime CreatedDate { get; set; }
    
    [Column("active")]
    public bool Active { get; set; }
    
    [Column("user_id")]
    public int UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User User{ get; set; }
    
    [Column("thread_type_id")]
    public int ThreadTypeId { get; set; }
    
    [ForeignKey(nameof(ThreadTypeId))]
    public ThreadType ThreadType { get; set; }
    
    public List<Post> Posts { get; set; }
}