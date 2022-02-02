namespace moosik.services.Dtos;

public class CreateThreadDto
{
    public string Title { get; set; }
    public int UserId { get; set; }
    public int ThreadTypeId { get; set; }
    public CreatePostDto Post { get; set; }
}