using Greeny.Common.Models;

namespace Greeny.Core.Contracts
{
    public interface ICategoryService
    {
        /// <summary>
        /// Получить список всех доступных категорий продукции
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CategoryModel>?> GetListAsync();
    }
}
