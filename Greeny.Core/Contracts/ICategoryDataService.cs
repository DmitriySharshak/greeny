using Greeny.Common.Models;

namespace Greeny.Core.Contracts
{
    public interface ICategoryDataService
    {
        /// <summary>
        /// Получить список всех доступных категорий продукции
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CategoryModel>> GetList();
    }
}
