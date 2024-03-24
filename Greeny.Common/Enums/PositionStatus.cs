namespace Greeny.Common.Enums
{
    /// <summary>
    /// Статусная модель позиции в заказе
    /// </summary>
    public enum PositionStatus
    {
        [Description("Новый")]
        New = 1,

        [Description("Подбор фермера")]
        FarmerStart,

        [Description("Фермер подобран")]
        FarmerFinished,

        [Description("Заказ принят")]
        FarmerSuccess,

        [Description("Сборка")]
        FarmerAssemblyStart,

        [Description("Сборка завершена")]
        FarmerAssemblyFinished,

        [Description("Водитель забрал заказ")]
        Driver
    }
}
