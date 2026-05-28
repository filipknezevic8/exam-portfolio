using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Exam.App.Domain;
using Exam.App.Services.Dtos;
using Exam.App.Services.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Exam.App.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IMapper mapper)
    {
        _userManager = userManager;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task Register(RegistrationDto data)
    {
        var user = new ApplicationUser
        {
            UserName = data.Username,
            Email = data.Email,
            Name = data.Name,
            Surname = data.Surname
        };

        var result = await _userManager.CreateAsync(user, data.Password);
        if (!result.Succeeded)
        {
            string errorMessage = string.Join(" ", result.Errors.Select(e => e.Description));
            throw new InvalidRegistrationException(errorMessage);
        }

        await _userManager.AddToRoleAsync(user, "User");
    }

    public async Task<string> Login(LoginDto data)
    {
        var user = await _userManager.FindByNameAsync(data.Username);
        if (user == null)
        {
            throw new InvalidCredentialsException("Invalid credentials.");
        }

        if (!await _userManager.CheckPasswordAsync(user, data.Password))
        {
            throw new InvalidCredentialsException("Invalid credentials.");
        }

        string token = await GenerateJwtToken(user);
        return token;
    }

    private async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserName),
            new(ClaimTypes.NameIdentifier, user.UserName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(180),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<ProfileDto> GetProfile(ClaimsPrincipal userPrincipal)
    {
        var username = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (username == null)
        {
            throw new InvalidCredentialsException("Invalid token");
        }

        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            throw new NotFoundException(0);
        }

        return _mapper.Map<ProfileDto>(user);
    }
}