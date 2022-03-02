using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using moosik.dal.Interfaces;
using moosik.dal.Models;

namespace moosik.dal.Contexts;
[ExcludeFromCodeCoverage]
public class MoosikContext : BaseContext, IMoosikDatabase
{
    public MoosikContext(string connectionString) : base(connectionString){}
    public DbSet<ThreadType> ThreadTypes { get; set; }
    public DbSet<PostResource> PostResources { get; set; }
    public DbSet<ResourceType> ResourceTypes { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Thread> Threads { get; set; }
    public DbSet<User> Users { get; set; }
    
}





