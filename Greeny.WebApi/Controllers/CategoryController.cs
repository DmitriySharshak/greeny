using Greeny.Common.Models;
using Greeny.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Greeny.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/categories")]
    [Produces("application/json")]
    public class CategoryController: ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet()]
        public async Task<IEnumerable<CategoryModel>?> Get()
        {
            return await _categoryService.GetRootAsync();
        }

        [HttpGet("{id}/children")]
        public async Task<IEnumerable<CategoryModel>?> GetSubCategories(long id)
        {
            return await _categoryService.GetChildrenAsync(id);
        }
    }
}
