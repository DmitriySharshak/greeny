namespace Greeny.Common.Models
{
    /// <summary>
    /// Пользователь сервиса
    /// </summary>
    public sealed class UserModel
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Полное имя пользователя
        /// </summary>
        public string FullName { get; set; }

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
