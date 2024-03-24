namespace Greeny.Common.Enums
{
    /// <summary>
    /// Статусная модель заказа
    /// </summary>
    public enum OrderStatus
    {
        [Description("Поступил новый заказ")]
        New = 1,

        [Description("Принят в работу")]
        Start,

        [Description("Подбор фермеров")]
        FarmerStart,

        [Description("Фермеры подобраны")]
        FarmerFinished,

        [Description("Фермеры приняли заказ")]
        FarmerSuccess,

        [Description("Водитель подобран")]
        Driver,

        [Description("Предварительная цена расчитана")]
        PriceStart,

        [Description("Оплачен")]
        Payment,

        [Description("Фермеры приступили к сборке")]
        FarmerAssembly,

        [Description("Фермеры собрали заказ")]
        FarmerPositionFinished,

        [Description("Итоговая цена сформирована")]
        PriceFinished,

        [Description("Разница возвращена клиенту")]
        PriceReturn,

        [Description("Водитель забрал заказ")]
        DriverStart,

        [Description("Заказ в пути")]
        DriverMove,

        [Description("Заказ принят клиентом")]
        Client,

        [Description("Заказ выполнен")]
        Finished
    }
}
