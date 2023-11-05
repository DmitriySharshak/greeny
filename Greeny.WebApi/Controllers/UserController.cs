using Greeny.Common.Models;
using Greeny.Core.Contract;
using Microsoft.AspNetCore.Mvc;

namespace Greeny.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet(Name = "user")]
    public async Task<UserModel?> Get(long id)
    {
        return await _userService.GetAsync(id);
    }

    //[HttpGet(Name = "userssss")]
    //public async Task<IEnumerable<UserModel>> List()
    //{
    //    return await _userService.ListAsync();
    //}


}