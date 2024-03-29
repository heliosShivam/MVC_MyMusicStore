using Microsoft.EntityFrameworkCore;
using MVC_MyMusicStore.Data;
using System;
using Microsoft.AspNetCore.Identity;
using MVC_MyMusicStore.Models;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
                   builder.Configuration.GetConnectionString("Default")
                   )
               );


        /*builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppDbContext>();
        */

        builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            // Disable lockout feature entirely
            options.Lockout.AllowedForNewUsers = false;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.Zero; // Lockout duration
            options.Lockout.MaxFailedAccessAttempts = 0; // Number of failed attempts before lockout
        });


        builder.Services.AddSession(options =>
        {
            options.Cookie.Name = "YourApp.Session";
            // Set a short timeout for easy testing.
            options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust as needed
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy =>
                policy.RequireUserName("admin@admin.com"));
        });
        builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromMinutes(5);
        });


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseSession();
        app.UseRouting();
        app.UseAuthorization();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        

        app.Run();
    }
}