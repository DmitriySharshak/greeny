using Greeny.Common.Models;
using Greeny.Core.Contract;
using Microsoft.AspNetCore.Mvc;

namespace Greeny.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserDataService _userDataService;
    public UserController(IUserDataService userDataService)
    {
        _userDataService = userDataService;
    }

    [HttpGet(Name = "users")]
    public IEnumerable<UserModel> GetList()
    {
        return _userDataService.GetAll();
    }


}