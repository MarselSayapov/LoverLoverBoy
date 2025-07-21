namespace Services.Models.User.Requests;

public class UserRequest
{
    public string? Login { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
}