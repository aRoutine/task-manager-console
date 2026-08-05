using TaskManager.Results;

namespace TaskManager.Interfaces;

public interface IAuthService
{
    Task<LoginResult> LoginAsync(string email, string password);
    Task<AuthOperationResult> RegisterAsync(string userName, string email, string password);
}