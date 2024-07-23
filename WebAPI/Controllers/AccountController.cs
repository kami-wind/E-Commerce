using BusinessObjects.Entites;
using DataAccess;
using DataAccess.DTOs;
using DataAccess.DTOs.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace WebAPI.Controllers;

[Route("odata/Accounts")]
[ApiController]
public class AccountController : ODataController
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var userDto = new UserDTO
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email
        };

        return Ok(userDto);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Validate username format
        if (!IsValidUsername(model.Username))
        {
            ModelState.AddModelError("Username", "Username can only contain letters or digits.");
            return BadRequest(ModelState);
        }

        var user = new User
        {
            UserName = model.Username,
            Email = model.Email,
            
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok();
    }

    [HttpPost("login")]
    public async Task<ActionResult<User>> Login([FromBody] LoginDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _signInManager.PasswordSignInAsync(model.Username,
                                                        model.Password,
                                                       isPersistent: false, 
                                                      lockoutOnFailure:  false);
        if (!result.Succeeded)
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByNameAsync(model.Username);

        //await _signInManager.SignInAsync(user, isPersistent: false);

        var token = CreateJwtToken(user);
        return Ok(token);

        //return Ok(new { token, email = user.Email, personName = user.UserName });
    }

    //private string GenerateJwtToken(User user)
    //{

    //    var claims = new[]
    //    {
    //            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
    //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
    //            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    //        };

    //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
    //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //    var token = new JwtSecurityToken(
    //        issuer: _configuration["Jwt:Issuer"],
    //        audience: _configuration["Jwt:Audience"],
    //        claims: claims,
    //        expires: DateTime.Now.AddDays(30),
    //        signingCredentials: creds
    //    );

    //    return new JwtSecurityTokenHandler().WriteToken(token);

    //}

    private AuthenticationResponse CreateJwtToken(User user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"]));

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject (user id)
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT Unique ID 
        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()), // Issued at (date and time of token generation)
        new Claim(ClaimTypes.NameIdentifier, user.Email), // Unique name identifier of the user (Email)
        new Claim(ClaimTypes.Name, user.UserName) // Name of the User
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenString = tokenHandler.WriteToken(token);

        return new AuthenticationResponse
        {
            Token = tokenString,
            Email = user.Email,
            PersonName = user.UserName, // Change to user.UserName since PersonName doesn't exist in User class
            Expiration = expiration
        };
    }

    private bool IsValidUsername(string username)
    {
        // Implement your custom validation logic here
        // Example: Check if username contains only letters and digits
        return Regex.IsMatch(username, @"^[a-zA-Z0-9]+$");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Fetch the existing user from the database
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return NotFound();
        }

        // Update user properties
        user.UserName = model.Username;
        user.Email = model.Email;

        // Ensure that the SecurityStamp is not null
        if (string.IsNullOrEmpty(user.SecurityStamp))
        {
            return BadRequest("SecurityStamp is missing.");
        }

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();
    }




    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();
    }

    [HttpGet("logout")]
    public async Task<ActionResult<User>> GetLogout()
    {
        await _signInManager.SignOutAsync();

        return NoContent();
    }
}
