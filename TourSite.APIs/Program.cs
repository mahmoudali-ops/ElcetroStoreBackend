
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using TourSite.APIs.Errors;
using TourSite.APIs.Helper;
using TourSite.APIs.MidleWare;
using TourSite.Core;
using TourSite.Core.Entities;
using TourSite.Core.Servicies.Contract;
using TourSite.Repository.Data;
using TourSite.Repository.Data.Contexts;
using TourSite.Repository.Repositories;


namespace TourSite.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];



            // ✅ CORS مفتوحة لأي حد (مؤقتًا)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("OpenCors", policy =>
                {
                    policy
                        .WithOrigins(
                            "https://electro-pi-five.vercel.app",
                            "http://localhost:4200",
                            "https://www.bbesocial.com" // 👈 ضيف دي

                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials(); // 👈 مهم جدًا
                });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ===== DI =====
            builder.Services.AddDependency(builder.Configuration);

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // =========================
                // 1. Ensure Roles exist
                // =========================
                string[] roles = { "Admin", "Customer" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // =========================
                // 2. Create Admin User
                // =========================
                var adminEmail = "admin@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    var user = new User
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        FullName = "System Admin"
                    };

                    var result = await userManager.CreateAsync(user, "Admin@123");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                }
                else
                {
                    // لو موجود بس ملوش Role
                    if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }
            }
            await app.UseConfigurationMiddleWare();


            // ===== Middlewares Order (الترتيب الصح) =====
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("OpenCors"); // 👈 قبل Auth

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseMiddleware<ExceptionMidleWare>();

            app.MapControllers();

            app.Run();
        }
    }
}
