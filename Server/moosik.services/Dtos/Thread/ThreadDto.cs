using System;

namespace moosik.services.Dtos;

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