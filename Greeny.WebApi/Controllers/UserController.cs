using Greeny.Common.Models;
using Greeny.Core.Contract;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Greeny.WebApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/users")]
[Produces(MediaTypeNames.Application.Json)]
[ApiVersion("1.0")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserModel?>> Login(string login, string password)
    {
        return await _userService.GetAsync(login, password);
    }

    [HttpPost("register")]
    public async Task<ActionResult<bool>> Register(UserRegisterModel user)
    {
        return await _userService.AddAsync(user);
    }
}

