using Greeny.Common.Enums;

namespace Greeny.Core.Validation
{
    public enum ValidatorResultCode
    {
        /// <summary>
        /// Поле \"{0}\" обязательно для заполнения.
        /// </summary>
        [Description("Поле \"{0}\" обязательно для заполнения.")]
        Required = 1,

        /// <summary>
        /// Общее количество символов не должно превышать {0}.
        /// </summary>
        [Description("Общее количество символов не должно превышать {0}.")]
        MaxLength = 2,

        /// <summary>
        /// Значение должно быть уникальным.
        /// </summary>
        [Description("Значение должно быть уникальным.")]
        Unique = 3,
    }
}
