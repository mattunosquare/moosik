using System;
using System.Collections.Generic;

namespace moosik.services.Dtos;

public class ThreadDto
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public int ThreadTypeId { get; set; }
    
    public ThreadTypeDto ThreadType { get; set; }
    
    public int UserId { get; set; }
    public UserDto User { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool Active { get; set; }
    public IEnumerable<PostDto> Posts { get; set; }
}