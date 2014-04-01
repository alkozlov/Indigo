namespace Indigo.DataAccessLayer
{
    using System.Data.Entity;

    using Indigo.DataAccessLayer.Models;
    
    public class IndigoDataContext : DbContext
    {
        public DbSet<UserAccount> UserAccounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region UserAccount

            modelBuilder.Entity<UserAccount>().HasKey(x => x.UserId);
            modelBuilder.Entity<UserAccount>().Property(x => x.Login).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<UserAccount>().Property(x => x.Email).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<UserAccount>().Property(x => x.Password).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<UserAccount>().Property(x => x.PasswordSalt).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<UserAccount>().Property(x => x.CreatedDateUtc).IsRequired();
            modelBuilder.Entity<UserAccount>().Property(x => x.LastLoginDateUtc).IsOptional();
            modelBuilder.Entity<UserAccount>().Property(x => x.RemovedDateUtc).IsOptional();
            modelBuilder.Entity<UserAccount>().Property(x => x.AccountType).IsRequired();
            modelBuilder.Entity<UserAccount>().Property(x => x.IsActive).IsRequired();

            #endregion

        }
    }
}