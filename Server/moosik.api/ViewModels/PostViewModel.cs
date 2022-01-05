﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace moosik.api.ViewModels;

public class PostViewModel
{
    public int Id { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    
    public UserViewModel User { get; set; }
    public int ThreadId { get; set; }
    
    public ThreadViewModel Thread { get; set; }

    public DateTime CreatedDate { get; set; }
    
    public bool Active { get; set; }
    
    public IEnumerable<PostResourceViewModel> PostResources { get; set; }
}