namespace Indigo.DataAccessLayer
{
    using System.Data.Entity;

    using Indigo.DataAccessLayer.Models;
    
    public class IndigoDataContext : DbContext
    {
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionAccessType> PermissionAccessTypes { get; set; }
        public DbSet<AccountPermission> AccountPermissions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new IndigoCreateDatabaseIfNotExists());

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

            #region Permission

            modelBuilder.Entity<Permission>().HasKey(x => x.PermissionType);
            modelBuilder.Entity<Permission>().Property(x => x.Description).IsRequired().HasMaxLength(100);

            #endregion

            #region PermissionAccessType

            modelBuilder.Entity<PermissionAccessType>().HasKey(x => x.AccessType);
            modelBuilder.Entity<PermissionAccessType>().Property(x => x.Description).IsRequired().HasMaxLength(100);

            #endregion

            #region AccountPermission

            modelBuilder.Entity<AccountPermission>().HasKey(x => new {x.AccountType, x.PermissionType, x.AccessType});
            modelBuilder.Entity<AccountPermission>().Property(x => x.LastModifiedDateUtc).IsRequired();
            modelBuilder.Entity<AccountPermission>().HasOptional(x => x.UserAccount).WithMany().HasForeignKey(x => x.ModifiedByUserId);

            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}