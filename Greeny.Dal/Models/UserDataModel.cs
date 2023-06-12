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
        [Column(Name = "phone_number", CanBeNull = false)]
        [Description("Номер телефона")]
        public virtual string PhoneNumber { get; set; }

        [NotNull]
        [Column(Name = "email", CanBeNull = false)]
        [Description("Адрес электронной почты")]
        public virtual string Email { get; set; }

        [NotNull]
        [Column(Name = "full_name", CanBeNull = false)]
        [Description("Полное имя")]
        public virtual string FullName { get; set; }

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
