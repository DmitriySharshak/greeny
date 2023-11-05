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
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("list")]
        public async Task<IEnumerable<CategoryModel>?> GetList()
        {
            return await _categoryService.GetListAsync();
        }
    }
}
