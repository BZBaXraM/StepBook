namespace StepBook.API.DTOs;

public class CreateMessageRequestDto
{
    public string RecipientUsername { get; set; } = null!;
    public string Content { get; set; } = null!;
}