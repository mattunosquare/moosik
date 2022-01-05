﻿namespace moosik.api.ViewModels;

public class PostResourceViewModel
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public string Value { get; set; }
    
    public int PostId { get; set; }

    public PostViewModel Post { get; set; }

    public int ResourceTypeId {get; set; }

    public ResourceTypeViewModel ResourceType { get; set; }
}