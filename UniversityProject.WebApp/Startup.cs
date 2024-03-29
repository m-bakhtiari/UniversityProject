using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UniversityProject.Core.Repositories;
using UniversityProject.Core.Services;
using UniversityProject.Data.Context;

namespace UniversityProject.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            #region Database

            services.AddDbContext<UniversityProjectContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("UniversityProjectConnection"));
                options.EnableSensitiveDataLogging();
            }, ServiceLifetime.Transient);

            #endregion

            #region IoC

            services.AddTransient<ICategoryRepository, CategoryService>();
            services.AddTransient<IBookRepository, BookService>();
            services.AddTransient<IBookCategoryRepository, BookCategoryService>();
            services.AddTransient<ISliderRepository, SliderService>();
            services.AddTransient<IBannerRepository, BannerService>();
            services.AddTransient<ICommentRepository, CommentService>();
            services.AddTransient<IUserBookRepository, UserBookService>();
            services.AddTransient<IUserRepository, UserService>();
            services.AddTransient<IRoleRepository, RoleService>();
            services.AddTransient<IShoppingCartRepository, ShoppingCartService>();
            services.AddTransient<IFavoriteBookRepository, FavoriteBookService>();
            services.AddTransient<IMessageRepository, MessageService>();
            services.AddTransient<ITeamRepository, TeamService>();

            #endregion

            #region Authentication

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(options =>
            {
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);

            });

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Home/Error";
                    await next();
                }
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
