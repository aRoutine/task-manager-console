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

        return Created(string.Empty,response);
    }
}