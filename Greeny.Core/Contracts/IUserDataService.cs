using Greeny.Common.Models;

namespace Greeny.Core.Contract
{
    /// <summary>
    /// Контракт предоставляет методы для управления пользователями системы
    /// </summary>
    public interface IUserDataService
    {
        /// <summary>
        /// Получить информацию о пользвателе
        /// по его идентификатору
        /// </summary>
        /// <param name="id">идентификатор пользователя</param>
        /// <returns></returns>
        UserModel? Get(long id);


        /// <summary>
        /// Получить список всех пользователей
        /// </summary>
        /// <returns></returns>
        IEnumerable<UserModel> GetAll();
    }
}
