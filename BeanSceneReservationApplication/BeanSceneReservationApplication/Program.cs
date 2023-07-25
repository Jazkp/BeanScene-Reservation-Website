using BeanSceneReservationApplication.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BeanSceneReservationApplication.Services;
using BeanSceneReservationApplication.Models.APIModel;
using Microsoft.AspNetCore.Mvc;

namespace BeanSceneReservationApplication
{
    public class Program
    {
        //async Main method must return 'Task' instead of 'void'
        public static async Task Main(string[] args)
        {

            //Pulling MongoDB
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
            builder.Services.AddSingleton<ProductDBService>();
            builder.Services.AddTransient<OrderDBService>();
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });


            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            //ALERT: Email confirmation has been set to false, it will be set back to true when email confirmation is properly set up
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapAreaControllerRoute(
                name: "management",
                areaName: "Management",
                pattern: "Management/{controller=Home}/{action=Index}/{id?}"
            );

            app.MapAreaControllerRoute(
                name: "staff",
                areaName: "Staff",
                pattern: "Staff/{controller=Home}/{action=Index}/{id?}"
            );

            app.MapAreaControllerRoute(
                name: "membership",
                areaName: "Membership",
                pattern: "Membership/{controller=Home}/{action=Index}/{id?}"
            );

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=RedirectUser}/{id?}"
            );
            
            app.MapRazorPages();

            //This hardcodes the roles "Manager, Staff, Member" by creating them if they don't exist at application startup
            using(var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[] { "Manager", "Staff", "Member" };

                foreach (var role in roles)
                {
                    if(!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }

            //This section hardcodes users by creating them if they don't exist at startup
            //Currently includes:
            //   -Owner account with Manager role
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                string email = "owner@beanscene.com";
                string password = "Abc123?";

                if(await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new IdentityUser();
                    user.UserName = email;
                    user.Email = email;
                    user.EmailConfirmed = true;

                    await userManager.CreateAsync (user, password);

                    await userManager.AddToRoleAsync(user, "Manager");
                    await userManager.AddToRoleAsync(user, "Staff");

                }
            }

            app.UseCors(p =>
            {
                p.AllowAnyHeader();
                p.AllowAnyMethod();
                p.AllowAnyOrigin();
            });

            app.Run();
        }
    }
}