using LinqToDB.Mapping;
using System.ComponentModel;

namespace Greeny.Dal.Models
{
    [Table(Name = "user", Schema = GreenySchema.SchemaName)]
    [Description("")]
    public class UserDataModel
    {
        [PrimaryKey, Identity]
        [Column(Name = "id", CanBeNull = false)]
        [Description("Id")]
        public virtual long Id { get; set; }

        [NotNull]
        [Column(Name = "type")]
        [Description("тип")]
        public virtual int Type { get; set; }

        [NotNull]
        [Column(Name = "phone_number")]
        [Description("Номер телефона")]
        public virtual string? PhoneNumber { get; set; }

        [NotNull]
        [Column(Name = "email")]
        [Description("Адрес электронной почты")]
        public virtual string? Email { get; set; }

        [NotNull]
        [Column(Name = "first_name")]
        [Description("Имя")]
        public virtual string? FirstName { get; set; }

        [NotNull]
        [Column(Name = "last_name")]
        [Description("Фамилия")]
        public virtual string? LastName { get; set; }

        [Column(Name = "middle_name")]
        [Description("Отчество")]
        public virtual string? MiddleName { get; set; }
        

        [NotNull]
        [Column(Name = "address")]
        [Description("Адрес")]
        public virtual string? Address { get; set; }

        [NotNull]
        [Column(Name = "password_hash")]
        [Description("Пароль")]
        public virtual string PasswordHash { get; set; }

        [NotNull]
        [Column(Name = "register_time")]
        [Description("Дата регистрации")]
        public virtual DateTime RegisterTime { get; set; }

        [Column(Name = "is_active")]
        [Description("Признак активности пользователя")]
        public virtual bool IsActive { get; set; }
    }
}
