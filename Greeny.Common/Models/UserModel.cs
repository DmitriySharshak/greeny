using Greeny.Common.Enums;

namespace Greeny.Common.Models
{
    /// <summary>
    /// Пользователь сервиса
    /// </summary>
    public sealed class UserModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Тип 
        /// </summary>
        public UserType Type { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }
        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Дата регистрации
        /// </summary>
        public DateTime RegisterTime { get; set; }
    }
}
