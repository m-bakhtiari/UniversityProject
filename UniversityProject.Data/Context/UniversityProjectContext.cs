using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using UniversityProject.Data.Consts;
using UniversityProject.Data.Entities;

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
        public DbSet<UserBook> UsersBook { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<FavoriteBook> FavoriteBooks { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Team> Teams { get; set; }
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
            modelBuilder.Entity<FavoriteBook>().HasKey(x => new { x.UserId, x.BookId });

            modelBuilder.Entity<Role>().HasData(new List<Role>()
            {
                new Role(){Id = Const.AdminRoleId,Title = "Admin"},
                new Role(){Id = Const.UserRoleId,Title = "User"}
            });

            modelBuilder.Entity<User>().HasData(new List<User>()
            {
                new User()
                {
                    Name = "Admin",
                    IsDelete = false,
                    Password ="E1-0A-DC-39-49-BA-59-AB-BE-56-E0-57-F2-0F-88-3E",
                    Penalty = 0,
                    Phone = "01234567890",
                    RoleId = Const.AdminRoleId,
                    RegisterTime =new DateTime(2022,04,04),
                    Id = 1001
                },
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
