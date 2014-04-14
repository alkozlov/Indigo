namespace Indigo.DesktopClient.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    using Indigo.BusinessLogicLayer.Account;

    public class ActionList : ReadOnlyCollection<ActionList.Item>
    {
         public class Item
         {
             public String ActionName { get; set; }
             public PermissionType Permission { get; set; }
             public AccessType Access { get; set; }
         }

        public ActionList(IList<Item> list) : base(list)
        {
        }

        public static async Task<ActionList> GetAccountActionList(UserAccount user)
        {
            if (user != null && user.IsAcive)
            {
                Dictionary<PermissionType, AccessType> accountPermissions = await user.GetAccountPermissions();
                if (accountPermissions == null || accountPermissions.Count == 0)
                {
                    return null;
                }

                List<Item> actions = accountPermissions.Select(x => new Item
                {
                    Permission = x.Key,
                    Access = x.Value,
                    ActionName = GetActionName(x.Key)
                }).ToList();

                ActionList actionList = new ActionList(actions);
                return actionList;
            }

            return null;
        }

        #region Helpers

        private static String GetActionName(PermissionType permission)
        {
            String actionName = String.Empty;

            switch (permission)
            {
                case PermissionType.ReferenceInformation:
                {
                    actionName = "Справочная информация";
                } break;

                case PermissionType.DocumentsCollection:
                {
                    actionName = "База документов";
                } break;

                case PermissionType.ProfileInformation:
                {
                    actionName = "Мой профиль";
                } break;

                case PermissionType.UserDatabase:
                {
                    actionName = "База пользователей";
                } break;

                case PermissionType.Reports:
                {
                    actionName = "Отчеты";
                } break;
            }

            return actionName;
        }

        #endregion
    }
}