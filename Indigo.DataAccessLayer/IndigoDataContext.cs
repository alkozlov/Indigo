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
        public DbSet<Document> Documents { get; set; }
        public DbSet<Shingle> Shingles { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<StopWord> StopWords { get; set; }
        public DbSet<SubjectKeyWord> SubjectKeyWords { get; set; }
        public DbSet<DocumentKeyWord> DocumentKeyWords { get; set; }
        public DbSet<DocumentSubject> DocumentSubjects { get; set; }

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

            #region Document

            modelBuilder.Entity<Document>().HasKey(x => x.DocumentId);
            modelBuilder.Entity<Document>().Property(x => x.DocumentGuid).IsRequired();
            modelBuilder.Entity<Document>().Property(x => x.FileExtension).IsRequired().HasMaxLength(15);
            modelBuilder.Entity<Document>().Property(x => x.OriginalName).IsRequired().HasMaxLength(256);
            modelBuilder.Entity<Document>().Property(x => x.StoredName).IsRequired().HasMaxLength(256);
            modelBuilder.Entity<Document>().Property(x => x.AddedUserId).IsRequired();
            modelBuilder.Entity<Document>().Property(x => x.CreateDateUtc).IsRequired();
            modelBuilder.Entity<Document>().HasRequired(x => x.AddedUser).WithMany().HasForeignKey(x => x.AddedUserId);

            #endregion

            #region Shingle

            modelBuilder.Entity<Shingle>().HasKey(x => x.ShingleId);
            modelBuilder.Entity<Shingle>().HasRequired(x => x.Document).WithMany().HasForeignKey(x => x.DocumentId);
            modelBuilder.Entity<Shingle>().Property(x => x.ShingleSize).IsRequired();
            modelBuilder.Entity<Shingle>().Property(x => x.CheckSum).IsRequired();

            #endregion

            #region Subject

            modelBuilder.Entity<Subject>().HasKey(x => x.SubjectId);
            modelBuilder.Entity<Subject>().Property(x => x.SubjectHeader).IsRequired().HasMaxLength(100);

            #endregion

            #region StopWord

            modelBuilder.Entity<StopWord>().HasKey(x => x.StopWordId);
            modelBuilder.Entity<StopWord>().Property(x => x.Content).IsRequired().HasMaxLength(100);

            #endregion

            #region SubjectKeyWord

            modelBuilder.Entity<SubjectKeyWord>().HasKey(x => x.SubjectKeyWordId);
            modelBuilder.Entity<SubjectKeyWord>().Property(x => x.Word).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<SubjectKeyWord>().HasRequired(x => x.Subject).WithMany().HasForeignKey(x => x.SubjectId);

            #endregion

            #region DocumentKeyWord

            modelBuilder.Entity<DocumentKeyWord>().HasKey(x => x.DocumentKeyWordId);
            modelBuilder.Entity<DocumentKeyWord>().Property(x => x.Word).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<DocumentKeyWord>().HasRequired(x => x.Document).WithMany().HasForeignKey(x => x.DocumentId);

            #endregion

            #region DocumentSubject

            modelBuilder.Entity<DocumentSubject>().HasKey(x => new {x.DocumentId, x.SubjectId});
            modelBuilder.Entity<DocumentSubject>().HasRequired(x => x.Document).WithMany().HasForeignKey(x => x.DocumentId);
            modelBuilder.Entity<DocumentSubject>().HasRequired(x => x.Subject).WithMany().HasForeignKey(x => x.SubjectId);

            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}