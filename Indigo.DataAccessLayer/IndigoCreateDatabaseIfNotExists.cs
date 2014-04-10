namespace Indigo.DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;

    using Indigo.DataAccessLayer.Models;

    public class IndigoCreateDatabaseIfNotExists : CreateDatabaseIfNotExists<IndigoDataContext>
    {
        protected override void Seed(IndigoDataContext context)
        {
            #region Insert permissions

            List<Permission> permissions = new List<Permission>
            {
                new Permission
                {
                    PermissionType = 0,
                    Description = "Справочная информация"
                },

                new Permission
                {
                    PermissionType = 1,
                    Description = "База документов"
                },

                new Permission
                {
                    PermissionType = 2,
                    Description = "Информация профиля"
                },

                new Permission
                {
                    PermissionType = 3,
                    Description = "База данных пользователей"
                },

                new Permission
                {
                    PermissionType = 4,
                    Description = "Отчеты"
                }
            };

            context.Permissions.AddOrUpdate(permissions.ToArray());

            #endregion

            #region Insert Access types

            List<PermissionAccessType> accessTypes = new List<PermissionAccessType>
            {
                new PermissionAccessType
                {
                    AccessType = 0,
                    Description = "Доступ закрыт"
                },

                new PermissionAccessType
                {
                    AccessType = 1,
                    Description = "Только чтение"
                },

                new PermissionAccessType
                {
                    AccessType = 2,
                    Description = "Чтение и запись"
                }
            };

            context.PermissionAccessTypes.AddOrUpdate(accessTypes.ToArray());

            #endregion

            #region Account permissions

            DateTime firstDraftDateUtc = DateTime.UtcNow;

            // 0 - Unknown
            // 1 - Admin
            // 2 - Lecturer
            // 3 - Viewer
            List<AccountPermission> accountPermissions = new List<AccountPermission>
            {
                #region Unknown
                
                new AccountPermission
                {
                    AccountType = 0,
                    PermissionType = 0,
                    AccessType = 0,
                    LastModifiedDateUtc = firstDraftDateUtc
                },

                new AccountPermission
                {
                    AccountType = 0,
                    PermissionType = 1,
                    AccessType = 0,
                    LastModifiedDateUtc = firstDraftDateUtc
                },

                new AccountPermission
                {
                    AccountType = 0,
                    PermissionType = 2,
                    AccessType = 0,
                    LastModifiedDateUtc = firstDraftDateUtc
                },

                new AccountPermission
                {
                    AccountType = 0,
                    PermissionType = 3,
                    AccessType = 0,
                    LastModifiedDateUtc = firstDraftDateUtc
                },

                new AccountPermission
                {
                    AccountType = 0,
                    PermissionType = 4,
                    AccessType = 0,
                    LastModifiedDateUtc = firstDraftDateUtc
                },

                #endregion

                #region Admin
                
                new AccountPermission
                {
                    AccountType = 1,
                    PermissionType = 0,
                    AccessType = 2,
                    LastModifiedDateUtc = firstDraftDateUtc
                },

                new AccountPermission
                {
                    AccountType = 1,
                    PermissionType = 1,
                    AccessType = 2,
                    LastModifiedDateUtc = firstDraftDateUtc
                },

                new AccountPermission
                {
                    AccountType = 1,
                    PermissionType = 2,
                    AccessType = 2,
                    LastModifiedDateUtc = firstDraftDateUtc
                },

                new AccountPermission
                {
                    AccountType = 1,
                    PermissionType = 3,
                    AccessType = 2,
                    LastModifiedDateUtc = firstDraftDateUtc
                },

                new AccountPermission
                {
                    AccountType = 1,
                    PermissionType = 4,
                    AccessType = 2,
                    LastModifiedDateUtc = firstDraftDateUtc
                },

                #endregion

                #region Lecturer
                
                new AccountPermission
                {
                    AccountType = 2,
                    PermissionType = 0,
                    AccessType = 2,
                    LastModifiedDateUtc = firstDraftDateUtc
                },

                new AccountPermission
                {
                    AccountType = 2,
                    PermissionType = 1,
                    AccessType = 2,
                    LastModifiedDateUtc = firstDraftDateUtc
                },

                new AccountPermission
                {
                    AccountType = 2,
                    PermissionType = 2,
                    AccessType = 2,
                    LastModifiedDateUtc = firstDraftDateUtc
                },

                new AccountPermission
                {
                    AccountType = 2,
                    PermissionType = 3,
                    AccessType = 0,
                    LastModifiedDateUtc = firstDraftDateUtc
                },

                new AccountPermission
                {
                    AccountType = 2,
                    PermissionType = 4,
                    AccessType = 2,
                    LastModifiedDateUtc = firstDraftDateUtc
                },

                #endregion

                #region Viewer
                
                new AccountPermission
                {
                    AccountType = 3,
                    PermissionType = 0,
                    AccessType = 1,
                    LastModifiedDateUtc = firstDraftDateUtc
                },

                new AccountPermission
                {
                    AccountType = 3,
                    PermissionType = 1,
                    AccessType = 1,
                    LastModifiedDateUtc = firstDraftDateUtc
                },

                new AccountPermission
                {
                    AccountType = 3,
                    PermissionType = 2,
                    AccessType = 1,
                    LastModifiedDateUtc = firstDraftDateUtc
                },

                new AccountPermission
                {
                    AccountType = 3,
                    PermissionType = 3,
                    AccessType = 1,
                    LastModifiedDateUtc = firstDraftDateUtc
                },

                new AccountPermission
                {
                    AccountType = 3,
                    PermissionType = 4,
                    AccessType = 1,
                    LastModifiedDateUtc = firstDraftDateUtc
                }

                #endregion
            };

            context.AccountPermissions.AddOrUpdate(accountPermissions.ToArray());

            #endregion

            base.Seed(context);
        }
    }
}
