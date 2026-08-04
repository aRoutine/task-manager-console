using Microsoft.AspNetCore.Identity;
using TaskManager.Data;
using TaskManager.Interfaces;
using TaskManager.Models;
using TaskManager.Results;

namespace TaskManager.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthOperationResult> RegisterAsync(string userName, string email, string password)
    {
        userName = userName.Trim();
        email = email.Trim().ToLowerInvariant();

        User? userWithEmail = await _userRepository.GetByEmailAsync(email);
        if (userWithEmail is not null)
        {
            return AuthOperationResult.Fail("Пользователь с таким email уже существует");
        }

        User? userWithName = await _userRepository.GetByUserNameAsync(userName);
        if (userWithName is not null)
        {
            return AuthOperationResult.Fail("Пользователь с таким именем уже существует");
        }

        User user = new User
        {
            UserName = userName,
            Email = email,
            CreatedAt = DateTime.UtcNow
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, password);

        _userRepository.Add(user);

        return AuthOperationResult.Ok("Регистрация прошла успешна", user.Id);
    }
}