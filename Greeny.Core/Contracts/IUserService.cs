using Greeny.Common.Models;

namespace Greeny.Core.Contract
{
    /// <summary>
    /// Контракт предоставляет методы для управления пользователями системы
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<UserModel?> GetAsync(string login, string password);

        /// <summary>
        /// Добавление нового пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> AddAsync(UserRegisterModel user);
    }
}
