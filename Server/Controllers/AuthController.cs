﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Server.DataBase;
using Server.Entities.Identity;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IApplicationDbContext _context;

    public AuthController(IConfiguration configuration, IApplicationDbContext context)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _context = context;
    }

    [HttpPost("register")]
    public async Task<ActionResult<int>> Register(UserDto request)
    {
        if (_context.Users.Any(u => u.Username == request.Username))
        {
            return BadRequest("Username already exists. Please choose a different username.");
        }

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User()
        {
            Username = request.Username,
            PasswordHash = passwordHash
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        int userId = user.Id;

        string token = CreateToken(user);

        return Ok(new { UserId = userId, Token = token });
    }



    [HttpPost("login")]
    public ActionResult<string> Login(UserDto request)
    {
        User? user = _context.Users.FirstOrDefault(x => x.Username == request.Username);

        if (user is null)
        {
            return BadRequest("User not found");
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            return BadRequest("Invalid password");
        }

        string token = CreateToken(user);

        return Ok(token);
    }

    private string CreateToken(User user)
    {
        var jwt = "";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("Jwt:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: new List<Claim>
                {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                },
                expires: DateTime.Now.AddDays(3),
                signingCredentials: creds);

            jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}
