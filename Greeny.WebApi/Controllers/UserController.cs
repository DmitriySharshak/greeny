using Greeny.Common.Models;
using Greeny.Core.Contract;
using Microsoft.AspNetCore.Mvc;

namespace Greeny.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly IUserDataService _userDataService;
    public UserController(IUserDataService userDataService)
    {
        _userDataService = userDataService;
    }

    [HttpGet(Name = "user")]
    public async Task<UserModel?> Get(long id)
    {
        return await _userDataService.GetAsync(id);
    }

    //[HttpGet(Name = "userssss")]
    //public async Task<IEnumerable<UserModel>> List()
    //{
    //    return await _userDataService.ListAsync();
    //}


}