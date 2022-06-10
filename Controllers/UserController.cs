
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using logapp.DTOs;
using logapp.Models;
using logapp.Repositories;
using logapp.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace logapp.Controllers;


[ApiController]
[Route("api/user")]


public class UserController : ControllerBase
{

    private readonly ILogger<UserController> _logger;
    private readonly IUserRepository _user;
    private readonly IConfiguration _config;

    public UserController(ILogger<UserController> logger,
    IUserRepository user, IConfiguration config)
    {
        _logger = logger;
        _user = user;

        _config = config;
    }



    [HttpGet]
    [Authorize]


    public async Task<ActionResult<List<UserDTO>>> GetAllUsers()
    {

        var IsSuperUser = User.Claims.FirstOrDefault(c => c.Type == UserConstants.IsSuperUser)?.Value;

        if (bool.Parse(IsSuperUser))
        {
            var usersList = await _user.GetAllUsers();

            // User -> UserDTO
            var dtoList = usersList.Select(x => x.asDto);

            return Ok(dtoList);
        }
        else
        {
            return BadRequest("You are not authorized to see all users");
        }



    }

    private int GetUserIdFromClaims(IEnumerable<Claim> claims)
    {
        return Convert.ToInt32(claims.Where(x => x.Type == UserConstants.Id).First().Value);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(
        [FromBody] UserLoginDTO Data
    )
    {



        // if(Status.Parse(bool)){}
        var existingUser = await _user.GetByEmail(Data.Email);

        if (existingUser.Status == false)
            return BadRequest("User not found");

        if (existingUser is null)
            return NotFound();

        if (!BCrypt.Net.BCrypt.Verify(Data.HashPassword, existingUser.HashPassword))
            return BadRequest("Username or password is incorrect");

        var token = Generate(existingUser);

        var res = new UserLoginResDTO
        {
            Id = existingUser.Id,
            Email = existingUser.Email,
            Token = token,
            IsSuperUser = existingUser.IsSuperUser,
            // Status = existingUser.Status

        };

        return Ok(res);
    }



    private string Generate(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(UserConstants.Id, user.Id.ToString()),
            new Claim(UserConstants.Email, user.Email),
            new Claim(UserConstants.Name, user.Name),
            new Claim(UserConstants.IsSuperUser, user.IsSuperUser.ToString()),
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }






    [HttpGet("{id}")]
    [Authorize] 

    public async Task<ActionResult<UserDTO>> GetUserById([FromRoute] int id)
    {

        var IsSuperUser = User.Claims.FirstOrDefault(c => c.Type == UserConstants.IsSuperUser)?.Value;
        var userId = User.Claims.FirstOrDefault(c => c.Type == UserConstants.Id)?.Value;
        // if (IsSuperUser.Trim().ToLower() == "true" || u)
        if (bool.Parse(IsSuperUser))
        {

            var res = await _user.GetUserById(id);

            if (res is null)
                return NotFound("No user found with given id");


            var dto = res.asDto;
            dto.ListOfTags = (await _user.GetUserTagsById(id)).Select(x => x.asDto).ToList();
            return Ok(dto);

        }
        else
        {
            return BadRequest("User can't see all users");
        }



    }
    [HttpPost]
    [Authorize]

    public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserCreateDTO Data)
    {
        // if (!(new string[] { "male", "female" }.Contains(Data.Gender.Trim().ToLower())))
        //     return BadRequest("Gender value is not recognized");

        //    / var subtractDate = DateTimeOffset.Now - Data.DateOfBirth;
        //     if (subtractDate.TotalDays / 365 < 18.0)
        //         return BadRequest("User must be at least 18 years old");
        var IsSuperUser = User.Claims.FirstOrDefault(c => c.Type == UserConstants.IsSuperUser)?.Value;

        if (bool.Parse(IsSuperUser))
        {
            var toCreateUser = new User
            {
                Name = Data.Name.Trim(),
                Email = Data.Email.Trim().ToLower(),
                HashPassword = BCrypt.Net.BCrypt.HashPassword(Data.HashPassword)
            };

            var createdUser = await _user.CreateUser(toCreateUser);
            return StatusCode(StatusCodes.Status201Created, createdUser.asDto);
        }
        else
        {
            return BadRequest("Only super user can create user");
        }




    }

    [HttpPut("{id}")]
    [Authorize]

    public async Task<ActionResult> UpdateUser([FromRoute] int id,
    [FromBody] UserUpdateDTO Data)
    {

        var IsSuperUser = User.Claims.FirstOrDefault(c => c.Type == UserConstants.IsSuperUser)?.Value;
        if (bool.Parse(IsSuperUser))
        {

            var existing = await _user.GetUserById(id);
            if (existing is null)
                return NotFound("No user found with given id");

            var toUpdateUser = existing with
            {
                Name = Data.Name?.Trim() ?? existing.Name,
                HashPassword = Data.HashPassword

            };

            var didUpdate = await _user.UpdateUser(toUpdateUser);

            if (!didUpdate)
                return StatusCode(StatusCodes.Status500InternalServerError, "Could not update user");

            return NoContent();
        }

        else
        {
            return BadRequest("Only superuser can update user datails");
        }

    }



    // [HttpDelete("{id}")]
    // public async Task<ActionResult> DeleteUser([FromRoute] int id)
    // {
    //     var existing = await _user.GetUserById(id);
    //     if (existing is null)
    //         return NotFound("No user found with given user name");

    //     var didDelete = _user.DeleteUser(id);

    //     return NoContent();
    // }
}