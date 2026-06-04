using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SB.Auth.Domain;
using SB.Auth.Domain.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SB.Auth.Application.UseCases;

public record LoginCommand
{
    public string Email { get; init; }
    public string Password { get; init; }
}

public class LoginUseCase(UserManager<ApplicationUser> userManager, JwtSettings jwtSettings)
{
    public async Task<Result<string>> Handle(LoginCommand command)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user is null)
            return Result.Fail("Invalid credentials");

        var passwordValid = await userManager.CheckPasswordAsync(user, command.Password);
        if (!passwordValid)
            return Result.Fail("Invalid credentials");

        var roles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new("employeeId", user.EmployeeId.ToString()),
        };

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(jwtSettings.ExpiresInHours),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
