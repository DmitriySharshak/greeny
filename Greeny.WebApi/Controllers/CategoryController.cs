using Greeny.Common.Models;
using Greeny.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Greeny.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryDataService _categoryDataService;
        public CategoryController(ICategoryDataService categoryDataService)
        {
            _categoryDataService = categoryDataService;
        }

        [HttpGet(Name = "list")]
        public async Task<IEnumerable<CategoryModel>> GetList()
        {
            return await _categoryDataService.GetList();
        }
    }
}
