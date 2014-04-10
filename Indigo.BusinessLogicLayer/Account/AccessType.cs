namespace Indigo.BusinessLogicLayer.Account
{
    using System.ComponentModel;

    public enum AccessType
    {
        [Description("Доступ закрыт")]
        None = 0,

        [Description("Только чтение")]
        Reader = 1,

        [Description("Чтение и запись")]
        Editor = 2
    }
}