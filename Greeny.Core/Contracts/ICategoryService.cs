using Greeny.Common.Models;

namespace Greeny.Core.Contracts
{
    public interface ICategoryService
    {
        /// <summary>
        /// Получить список категорий продукции верхнего уровня 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CategoryModel>> GetRootsAsync();

        /// <summary>
        /// Получить список вложенных категорий продукции
        /// </summary>
        /// <param name="id">Идентификатор категории</param>
        /// <returns></returns>
        Task<IEnumerable<CategoryModel>> GetDescendantsAsync(long id);
    }
}
