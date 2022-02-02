using System;
using System.Collections.Generic;

namespace moosik.services.Dtos;

public class PostDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public int ThreadId { get; set; }
    public UserDto User { get; set; }
    public PostResourceDto[] Resources { get; set; }
}