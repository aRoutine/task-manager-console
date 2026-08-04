using TaskManager.Results;

namespace TaskManager.Interfaces;

public interface IAuthService
{
    Task<AuthOperationResult> RegisterAsync(string userName, string email, string password);
        
}