﻿namespace moosik.services.Dtos;

public class PostResourceDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Value { get; set; }
    public int PostId { get; set; }
    public PostDto Post { get; set; }
    public int ResourceTypeId { get; set; }
    public ResourceTypeDto ResourceType { get; set; }
}