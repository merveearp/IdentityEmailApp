using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using IdentityEmailApp.Extensions;
using IdentityEmailApp.Models.JWTModels;
using IdentityEmailApp.Services.Abstract;
using IdentityEmailApp.Services.Concrete;
using IdentityEmailApp.Validator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();

builder.Services.AddDbContext<EmailContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services
    .AddIdentity<AppUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireDigit = true;
        options.Password.RequireNonAlphanumeric = true;
    })
    .AddEntityFrameworkStores<EmailContext>()
    .AddErrorDescriber<CustomErrorValidator>()
    .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(
        TokenOptions.DefaultProvider);

builder.Services.ConfigureService();

builder.Services.Configure<JWTSettingsViewModel>(
    builder.Configuration.GetSection("JwtSettingsKey"));

builder.Services
    .AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
    {
        var jwtSettings = builder.Configuration
            .GetSection("JwtSettingsKey")
            .Get<JWTSettingsViewModel>();

        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings!.Issuer,
            ValidAudience = jwtSettings.Audience,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });

var app = builder.Build();

app.UseStatusCodePagesWithReExecute("/Error/{0}");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Message}/{action=Inbox}/{id?}")
    .WithStaticAssets();

app.Run();