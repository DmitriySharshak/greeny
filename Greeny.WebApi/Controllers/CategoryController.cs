using Greeny.Common.Models;
using Greeny.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Greeny.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryDataService _categoryDataService;
        public CategoryController(ICategoryDataService categoryDataService)
        {
            _categoryDataService = categoryDataService;
        }

        [HttpGet("list")]
        public async Task<IEnumerable<CategoryModel>> GetList()
        {
            return await _categoryDataService.GetList();
        }
    }
}
