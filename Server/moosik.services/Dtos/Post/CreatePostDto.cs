using moosik.services.Dtos;

public class CreatePostDto
{
    public int ThreadId { get; set; }
    
    public int UserId { get; set; }

    public string Description { get; set; }
    
    public CreatePostResourceDto Resource { get; set; }
}