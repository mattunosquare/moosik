﻿using System.Collections.Generic;

namespace moosik.services.Dtos;

public class UserDto
{
    public int Id { get; set; }
    
    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public bool Active { get; set; }
    
    public IEnumerable<ThreadDto> Threads { get; set; }
    public IEnumerable<PostDto> Posts { get; set; }
}