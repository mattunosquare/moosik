using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8618

namespace moosik.dal.Contexts;

public class MoosikContext : DbContext
{
    private readonly string _connectionString;
    public MoosikContext(string connectionString) => _connectionString = connectionString;
    
    public DbSet<ThreadType> ThreadTypes { get; set; }
    public DbSet<PostResource> PostResources { get; set; }
    public DbSet<ResourceType> ResourceTypes { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Thread> Threads { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        
        optionsBuilder.UseNpgsql(_connectionString);
    }
}

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

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("username")]
    public string Username { get; set; }
    
    [Column("email")]
    public string Email { get; set; }
    
    [Column("active")]
    public bool Active { get; set; }

    public List<Thread> Threads { get; set; }
    
    public List<Post> Posts { get; set; }
}

