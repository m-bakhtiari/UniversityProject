using Microsoft.EntityFrameworkCore;
using System.Linq;
using UniversityProject.Domain.Entities;

namespace UniversityProject.Data.Context
{
    public class UniversityProjectContext : DbContext
    {

        public UniversityProjectContext(DbContextOptions<UniversityProjectContext> options) : base(options)
        {

        }


        #region Entities

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<SubscriptionType> SubscriptionTypes { get; set; }
        public DbSet<UserBook> UsersBook { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; } 
        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDelete);
            modelBuilder.Entity<Book>().HasQueryFilter(u => !u.IsDelete);

            modelBuilder.Entity<ShoppingCart>().HasKey(x => new { x.UserId, x.BookId });
            modelBuilder.Entity<BookCategory>().HasKey(x => new { x.BookId, x.CategoryId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
