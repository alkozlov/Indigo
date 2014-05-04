namespace Indigo.WinClient.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Drawing;
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
             public Brush BackgroundColor { get; set; }
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
                    ActionName = GetActionName(x.Key),
                    BackgroundColor = GetActionBackgroundColor(x.Key)
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
                    actionName = "Справочники";
                } break;

                case PermissionType.DocumentsCollection:
                {
                    actionName = "Документы";
                } break;

                case PermissionType.ProfileInformation:
                {
                    actionName = "Мой профиль";
                } break;

                case PermissionType.UserDatabase:
                {
                    actionName = "Пользователи";
                } break;

                case PermissionType.Reports:
                {
                    actionName = "Отчеты";
                } break;
            }

            return actionName;
        }

        private static Brush GetActionBackgroundColor(PermissionType permission)
        {
            Color backgroundColor = Color.FromArgb(206, 26, 55);

            switch (permission)
            {
                case PermissionType.ReferenceInformation:
                {
                    backgroundColor = Color.FromArgb(248, 90, 50);
                } break;

                case PermissionType.DocumentsCollection:
                {
                    backgroundColor = Color.FromArgb(148, 168, 12);
                } break;

                case PermissionType.ProfileInformation:
                {
                    backgroundColor = Color.FromArgb(206, 26, 55);
                } break;

                case PermissionType.UserDatabase:
                    {
                        backgroundColor = Color.FromArgb(2, 107, 193);
                    } break;

                case PermissionType.Reports:
                    {
                        backgroundColor = Color.FromArgb(102, 24, 136);
                    } break;
            }
            
            Brush backgroundBrush = new SolidBrush(backgroundColor);
            return backgroundBrush;
        }

        #endregion
    }
}