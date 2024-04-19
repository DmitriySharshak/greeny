using Greeny.Common.Enums;

namespace Greeny.Common.Models
{
    /// <summary>
    /// Модель регистрации пользователя
    /// </summary>
    public sealed class UserRegisterModel
    {
        /// <summary>
        /// Тип 
        /// </summary>
        public UserType Type { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string? MiddleName { get; set; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string? Address { get; set; }
    }
}
