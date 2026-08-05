using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Api.Configuration;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Api.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtOptions _jwtOptions;

    public JwtTokenService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public string GenerateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
          new Claim(
            ClaimTypes.NameIdentifier,
            user.Id.ToString()
          ),
          
          new Claim(
            ClaimTypes.Name,
            user.UserName
          ),

          new Claim(
            ClaimTypes.Email,
            user.Email
          )
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}