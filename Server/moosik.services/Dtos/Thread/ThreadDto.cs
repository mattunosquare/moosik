using System;
using System.Diagnostics.CodeAnalysis;
using moosik.services.Dtos.Post;

namespace moosik.services.Dtos.Thread;
[ExcludeFromCodeCoverage]
public class ThreadDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedDate { get; set; }
    public ThreadTypeDto ThreadType { get; set; }
    public int UserId { get; set; }
    public UserDto User { get; set; }
    public PostDto[] Posts { get; set; }
}