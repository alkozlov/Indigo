namespace Indigo.BusinessLogicLayer.Tests
{
    using System;
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
        }
    }
}