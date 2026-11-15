using AutoMapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEFController(IConfiguration config) : ControllerBase
{
    readonly DataContextEF _entityFramework = new(config);
    readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<UserDto, User>();
    }, NullLoggerFactory.Instance));

    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        IEnumerable<User> users = _entityFramework.Users.ToList<User>();
        return users;
    }

    [HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUser(int userId)
    {
        User? user = _entityFramework.Users
            .Where(u => u.UserId == userId)
            .FirstOrDefault<User>();

        if (user != null)
        {
            return user;
        }

        throw new Exception("User not found");
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        User? userDb = _entityFramework.Users
            .Where(u => u.UserId == user.UserId)
            .FirstOrDefault<User>();

        if (userDb != null)
        {
            userDb.Active = user.Active;
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;

            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }

            throw new Exception("Failed to update User");
        }

        throw new Exception("User not found");
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserDto user)
    {
        User userDb = _mapper.Map<User>(user);

        _entityFramework.Add(userDb);

        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }

        throw new Exception("Failed to create User");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _entityFramework.Users
            .Where(u => u.UserId == userId)
            .FirstOrDefault<User>();

        if (userDb != null)
        {
            _entityFramework.Users.Remove(userDb);

            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }

            throw new Exception("Failed to delte User");
        }

        throw new Exception("User not found");
    }
}
