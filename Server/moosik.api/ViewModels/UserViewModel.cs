using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace moosik.api.ViewModels;

public class UserViewModel
{
    public int Id { get; set; }
    
    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public bool Active { get; set; }
    
    public IEnumerable<ThreadViewModel> Threads { get; set; }
    
    public IEnumerable<PostViewModel> Posts { get; set; }
}

