using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace moosik.dal.Models;

[Table("posts")]
public class Post
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("description")]
    public string Description { get; set; }
    
    [Column("created_date")]
    public DateTime CreatedDate { get; set; }
    
    [Column("active")]
    public bool Active { get; set; }
    
    [Column("user_id")]
    public int UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
    
    [Column("thread_id")]
    public int ThreadId { get; set; }
    
    [ForeignKey(nameof(ThreadId))]
    public Thread Thread { get; set; }
    
    public List<PostResource> PostResources { get; set; }
}