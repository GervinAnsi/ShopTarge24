using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using ShopTarge24.ApplicationServices.Services;
using ShopTarge24.Core.ServiceInterface;
using ShopTarge24.Data;
using ShopTarge24.Hubs;
using ShopTarge24.Controllers;
using ShopTarge24.Core.Domain;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ISpaceshipServices, SpaceshipServices>();
builder.Services.AddScoped<IFileServices, FileServices>();
builder.Services.AddScoped<IRealEstateServices, RealEstateServices>();
builder.Services.AddScoped<IWeatherForecastServices, WeatherForecastServices>();
builder.Services.AddScoped<OpenWeatherServices>();
builder.Services.AddScoped<IKindergartenServices, KindergartenServices>();
builder.Services.AddSignalR();
builder.Services.AddScoped<IEmailServices, EmailServices>();

builder.Services.AddDbContext<ShopTarge24Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 3;
})
    .AddEntityFrameworkStores<ShopTarge24Context>()
    .AddDefaultTokenProviders()
    .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("CustomEmailConfirmation");
/*.AddDefaultUI()*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.UseStaticFiles();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapHub<UserHub>("/Hubs/userCount");


app.Run();
