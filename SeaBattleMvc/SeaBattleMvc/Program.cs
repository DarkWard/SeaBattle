using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SeaBattleMvc;
using SeaBattleORM;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "Auth";
                    options.LoginPath = new PathString("/Account/Login");
                    options.LogoutPath = new PathString("/Account/Logout");
                    options.ExpireTimeSpan = new TimeSpan(7, 0, 0, 0);
                });

builder.Services.AddIdentity<AppUser, AppRole>()
            .AddUserStore<CustomUserStore>()
            .AddRoleStore<CustomRoleStore>()
            .AddUserManager<UserManager<AppUser>>()
            .AddDefaultTokenProviders();

builder.Services.AddTransient<SqlConnection>(e => new SqlConnection(connectionString));
builder.Services.AddScoped<UnitOfWork<AppUser>>(e => new UnitOfWork<AppUser>(connectionString));
builder.Services.AddScoped<UnitOfWork<AppRole>>(a => new UnitOfWork<AppRole>(connectionString));

builder.Services.AddScoped<CustomUserStore>();
builder.Services.AddScoped<CustomRoleStore>();

builder.Services.AddMvc();

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
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();