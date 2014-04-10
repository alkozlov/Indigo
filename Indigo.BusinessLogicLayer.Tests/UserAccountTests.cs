namespace Indigo.BusinessLogicLayer.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Indigo.BusinessLogicLayer.Account;
    
    using NUnit.Framework;

    [TestFixture]
    public class UserAccountTests
    {
        [Test]
        public async Task CreateAdminUserAccountTest()
        {
            String login = Guid.NewGuid().ToString("N");
            String email = String.Format("{0}@test.indigo.com", login);
            String password = login;

            UserAccount userAccount = await UserAccount.CreateAsync(login, email, password, UserAccountType.Admin);
            Assert.IsNotNull(userAccount);
            Assert.AreEqual(login, userAccount.Login);
            Assert.AreEqual(email, userAccount.Email);
            Assert.IsNotNull(userAccount.Password);
            Assert.IsNotNull(userAccount.PasswordSalt);
            Assert.AreEqual(UserAccountType.Admin, userAccount.AccountType);
        }

        [Test]
        public async Task CreateLecturerUserAccountTest()
        {
            String login = Guid.NewGuid().ToString("N");
            String email = String.Format("{0}@test.indigo.com", login);
            String password = login;

            UserAccount userAccount = await UserAccount.CreateAsync(login, email, password, UserAccountType.Lecturer);
            Assert.IsNotNull(userAccount);
            Assert.AreEqual(login, userAccount.Login);
            Assert.AreEqual(email, userAccount.Email);
            Assert.IsNotNull(userAccount.Password);
            Assert.IsNotNull(userAccount.PasswordSalt);
            Assert.AreEqual(UserAccountType.Lecturer, userAccount.AccountType);
        }

        [Test]
        public async Task CreateViewerUserAccountTest()
        {
            String login = Guid.NewGuid().ToString("N");
            String email = String.Format("{0}@test.indigo.com", login);
            String password = login;

            UserAccount userAccount = await UserAccount.CreateAsync(login, email, password, UserAccountType.Viewer);
            Assert.IsNotNull(userAccount);
            Assert.AreEqual(login, userAccount.Login);
            Assert.AreEqual(email, userAccount.Email);
            Assert.IsNotNull(userAccount.Password);
            Assert.IsNotNull(userAccount.PasswordSalt);
            Assert.AreEqual(UserAccountType.Viewer, userAccount.AccountType);
        }

        [Test]
        public async Task GetAdminPermissionsTest()
        {
            String login = Guid.NewGuid().ToString("N");
            String email = String.Format("{0}@test.indigo.com", login);
            String password = login;

            UserAccount userAccount = await UserAccount.CreateAsync(login, email, password, UserAccountType.Admin);
            Assert.IsNotNull(userAccount);
            Assert.AreEqual(login, userAccount.Login);
            Assert.AreEqual(email, userAccount.Email);
            Assert.IsNotNull(userAccount.Password);
            Assert.IsNotNull(userAccount.PasswordSalt);
            Assert.AreEqual(UserAccountType.Admin, userAccount.AccountType);

            Dictionary<PermissionType, AccessType> accountPermissions = await userAccount.GetAccountPermissions();
            Assert.IsNotNull(accountPermissions);
            CollectionAssert.IsNotEmpty(accountPermissions);

            CollectionAssert.Contains(accountPermissions,
                new KeyValuePair<PermissionType, AccessType>(PermissionType.ReferenceInformation, AccessType.Editor),
                String.Format("Mismatch of rights for: {0}", PermissionType.ReferenceInformation));
            CollectionAssert.Contains(accountPermissions,
                new KeyValuePair<PermissionType, AccessType>(PermissionType.DocumentsCollection, AccessType.Editor),
                String.Format("Mismatch of rights for: {0}", PermissionType.DocumentsCollection));
            CollectionAssert.Contains(accountPermissions,
                new KeyValuePair<PermissionType, AccessType>(PermissionType.ProfileInformation, AccessType.Editor),
                String.Format("Mismatch of rights for: {0}", PermissionType.ProfileInformation));
            CollectionAssert.Contains(accountPermissions,
                new KeyValuePair<PermissionType, AccessType>(PermissionType.UserDatabase, AccessType.Editor),
                String.Format("Mismatch of rights for: {0}", PermissionType.UserDatabase));
            CollectionAssert.Contains(accountPermissions,
                new KeyValuePair<PermissionType, AccessType>(PermissionType.Reports, AccessType.Editor),
                String.Format("Mismatch of rights for: {0}", PermissionType.Reports));
        }

        [Test]
        public async Task GetLecturerPermissionsTest()
        {
            String login = Guid.NewGuid().ToString("N");
            String email = String.Format("{0}@test.indigo.com", login);
            String password = login;

            UserAccount userAccount = await UserAccount.CreateAsync(login, email, password, UserAccountType.Lecturer);
            Assert.IsNotNull(userAccount);
            Assert.AreEqual(login, userAccount.Login);
            Assert.AreEqual(email, userAccount.Email);
            Assert.IsNotNull(userAccount.Password);
            Assert.IsNotNull(userAccount.PasswordSalt);
            Assert.AreEqual(UserAccountType.Lecturer, userAccount.AccountType);

            Dictionary<PermissionType, AccessType> accountPermissions = await userAccount.GetAccountPermissions();
            Assert.IsNotNull(accountPermissions);
            CollectionAssert.IsNotEmpty(accountPermissions);

            CollectionAssert.Contains(accountPermissions,
                new KeyValuePair<PermissionType, AccessType>(PermissionType.ReferenceInformation, AccessType.Editor),
                String.Format("Mismatch of rights for: {0}", PermissionType.ReferenceInformation));
            CollectionAssert.Contains(accountPermissions,
                new KeyValuePair<PermissionType, AccessType>(PermissionType.DocumentsCollection, AccessType.Editor),
                String.Format("Mismatch of rights for: {0}", PermissionType.DocumentsCollection));
            CollectionAssert.Contains(accountPermissions,
                new KeyValuePair<PermissionType, AccessType>(PermissionType.ProfileInformation, AccessType.Editor),
                String.Format("Mismatch of rights for: {0}", PermissionType.ProfileInformation));
            CollectionAssert.Contains(accountPermissions,
                new KeyValuePair<PermissionType, AccessType>(PermissionType.UserDatabase, AccessType.None),
                String.Format("Mismatch of rights for: {0}", PermissionType.UserDatabase));
            CollectionAssert.Contains(accountPermissions,
                new KeyValuePair<PermissionType, AccessType>(PermissionType.Reports, AccessType.Editor),
                String.Format("Mismatch of rights for: {0}", PermissionType.Reports));
        }

        [Test]
        public async Task GetViewerPermissionsTest()
        {
            String login = Guid.NewGuid().ToString("N");
            String email = String.Format("{0}@test.indigo.com", login);
            String password = login;

            UserAccount userAccount = await UserAccount.CreateAsync(login, email, password, UserAccountType.Viewer);
            Assert.IsNotNull(userAccount);
            Assert.AreEqual(login, userAccount.Login);
            Assert.AreEqual(email, userAccount.Email);
            Assert.IsNotNull(userAccount.Password);
            Assert.IsNotNull(userAccount.PasswordSalt);
            Assert.AreEqual(UserAccountType.Viewer, userAccount.AccountType);

            Dictionary<PermissionType, AccessType> accountPermissions = await userAccount.GetAccountPermissions();
            Assert.IsNotNull(accountPermissions);
            CollectionAssert.IsNotEmpty(accountPermissions);

            CollectionAssert.Contains(accountPermissions,
                new KeyValuePair<PermissionType, AccessType>(PermissionType.ReferenceInformation, AccessType.Reader),
                String.Format("Mismatch of rights for: {0}", PermissionType.ReferenceInformation));
            CollectionAssert.Contains(accountPermissions,
                new KeyValuePair<PermissionType, AccessType>(PermissionType.DocumentsCollection, AccessType.Reader),
                String.Format("Mismatch of rights for: {0}", PermissionType.DocumentsCollection));
            CollectionAssert.Contains(accountPermissions,
                new KeyValuePair<PermissionType, AccessType>(PermissionType.ProfileInformation, AccessType.Reader),
                String.Format("Mismatch of rights for: {0}", PermissionType.ProfileInformation));
            CollectionAssert.Contains(accountPermissions,
                new KeyValuePair<PermissionType, AccessType>(PermissionType.UserDatabase, AccessType.Reader),
                String.Format("Mismatch of rights for: {0}", PermissionType.UserDatabase));
            CollectionAssert.Contains(accountPermissions,
                new KeyValuePair<PermissionType, AccessType>(PermissionType.Reports, AccessType.Reader),
                String.Format("Mismatch of rights for: {0}", PermissionType.Reports));
        }
    }
}