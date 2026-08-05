using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Contracts;
using TaskManager.Interfaces;
using TaskManager.Results;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        AuthOperationResult result = await _authService.RegisterAsync(
            request.UserName,
            request.Email,
            request.Password
        );

        if (!result.Succes)
        {
            return Conflict(new
            {
                result.Succes,
                result.Message
            });
        }

        AuthResponse response = new AuthResponse
        {
            UserId = result.UserId!.Value,
            UserName = request.UserName,
            Email = request.Email,
            Message = result.Message
        };

        return Created(string.Empty, response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        LoginResult result = await _authService.LoginAsync(request.Email, request.Password);

        if (!result.Success || result.User == null)
        {
            return Unauthorized(new { result.Success, result.Message });
        }

        AuthResponse response = new AuthResponse
        {
            Message = result.Message,
            UserName = result.User.UserName,
            Email = result.User.Email,
            UserId = result.User.Id,
            Token = result.Token
        };

        return Ok(response);
    }
}