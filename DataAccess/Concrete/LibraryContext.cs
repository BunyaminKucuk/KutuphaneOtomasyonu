using Entity.Concrete;
using Entity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class LibraryContext : IdentityDbContext<ApplicationUser,
    ApplicationRole, string, IdentityUserClaim<string>,
    ApplicationUserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>
    {

        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=localhost;Database=LibraryDb;User=root;Password=123456", new MySqlServerVersion(new Version(8, 0, 31)));

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.HasCharSet(null, DelegationModes.ApplyToDatabases);
            builder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<TakeOfBook>(takeOfBook =>
            {
                takeOfBook.HasKey(x => new { x.UserId, x.BookId });

                takeOfBook.HasOne(x => x.Book)
                    .WithMany(y => y.TakeOfBooks)
                    .HasForeignKey(x => x.Id)
                    .IsRequired();

                takeOfBook.HasOne(x => x.User)
                    .WithMany(y => y.TakeOfBooks)
                    .HasForeignKey(x => x.Id)
                    .IsRequired();
            });

            foreach (var foreignKey in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }


        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TakeOfBook> TakeOfBooks { get; set; }
    }
}
