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
        [Column(Name = "type", CanBeNull = false)]
        [Description("тип")]
        public virtual int Type { get; set; }

        [NotNull]
        [Column(Name = "phone_number", CanBeNull = false)]
        [Description("Номер телефона")]
        public virtual string PhoneNumber { get; set; }

        [NotNull]
        [Column(Name = "email", CanBeNull = false)]
        [Description("Адрес электронной почты")]
        public virtual string Email { get; set; }

        [NotNull]
        [Column(Name = "first_name", CanBeNull = false)]
        [Description("Имя")]
        public virtual string FirstName { get; set; }

        [NotNull]
        [Column(Name = "middle_name", CanBeNull = false)]
        [Description("Отчество")]
        public virtual string MiddleName { get; set; }

        [NotNull]
        [Column(Name = "last_name", CanBeNull = false)]
        [Description("Фамилия")]
        public virtual string LastName { get; set; }

        [NotNull]
        [Column(Name = "password", CanBeNull = false)]
        [Description("Пароль")]
        public virtual string Password { get; set; }

        [NotNull]
        [Column(Name = "register_time", CanBeNull = false)]
        [Description("Дата регистрации")]
        public virtual DateTime RegisterTime { get; set; }
    }
}
