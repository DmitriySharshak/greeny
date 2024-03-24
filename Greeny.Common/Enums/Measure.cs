namespace Greeny.Common.Enums
{

    /// <summary>
    /// Единица измерения
    /// </summary>
    public enum ProductMeasure
    {
        [Description("шт")]
        Count = 1,

        [Description("кг")]
        Weight,

        [Description("л")]
        Volume,
    }
}
