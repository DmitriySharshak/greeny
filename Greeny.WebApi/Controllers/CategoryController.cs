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

        /// <summary>
        /// Получить список корневых категорий товаров
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IEnumerable<CategoryModel>?> GetRoots()
        {
            return await _categoryService.GetRootsAsync();
        }

        /// <summary>
        /// Получить список потомков для родительской категории 
        /// </summary>
        /// <param name="id">Идентификатор родительской категории</param>
        /// <returns></returns>
        [HttpGet("{id}/descendants")]
        public async Task<IEnumerable<CategoryModel>?> GetDescendants(long id)
        {
            return await _categoryService.GetDescendantsAsync(id);
        }
    }
}
