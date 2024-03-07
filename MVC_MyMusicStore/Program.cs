using Microsoft.EntityFrameworkCore;
using MVC_MusicStore.Data;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MVC_MyMusicStore.Models;
using MVC_MyMusicStore.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
           builder.Configuration.GetConnectionString("Default")
           )
       );

builder.Services.AddIdentity<IdentityUser , IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeedData(scope.ServiceProvider);
}
    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();