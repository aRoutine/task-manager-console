namespace TaskManager.Api.Contracts;

public class AuthResponse
{
    public string Message { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string Token { get; set; } = string.Empty;
}