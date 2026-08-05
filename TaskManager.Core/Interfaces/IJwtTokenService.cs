using TaskManager.Models;

namespace TaskManager.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}