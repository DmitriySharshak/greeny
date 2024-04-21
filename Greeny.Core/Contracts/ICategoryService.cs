using Greeny.Common.Models;

namespace Greeny.Core.Contracts
{
    public interface ICategoryService
    {
        /// <summary>
        /// Получить список категорий с вычесленной hash суммой
        /// </summary>
        /// <returns></returns>
        Task<CategoryHash> GetListAsync(int hash);
    }
}
