using Greeny.Common.Models;
using Greeny.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Greeny.WebApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/categories")]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiVersion("1.0")]
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
        [HttpGet("{hash}")]
        public async Task<CategoryHash?> GetList(int hash)
        {
            return await _categoryService.GetListAsync(hash);
        }
    }
}
