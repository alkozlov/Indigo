namespace Indigo.BusinessLogicLayer.Account
{
    using System.ComponentModel;

    public enum PermissionType : byte
    {
        [Description("Справочная информация")]
        ReferenceInformation = 0,

        [Description("База документов")]
        DocumentsCollection = 1,

        [Description("Информация профиля")]
        ProfileInformation = 2,

        [Description("База данных пользователей")]
        UserDatabase = 3,

        [Description("Отчеты")]
        Reports = 4
    }
}