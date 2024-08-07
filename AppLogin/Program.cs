using AppLogin.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

/*Cadena de conexion sql*/ 
builder.Services.AddDbContext<AppDbContext>(options => {
	options.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSql"));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options => {
		options.LoginPath = "/Acces/Login";
		options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
	app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
		name: "default",
		pattern: "{controller=Access}/{action=Login}/{id?}");

app.Run();
