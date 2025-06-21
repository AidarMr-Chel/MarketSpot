using Serilog;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MarketSpot.Core.Entities;
using MarketSpot.Application.Interfaces;
using MarketSpot.Infrastructure;
using MarketSpot.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account/login"; 
    options.AccessDeniedPath = "/account/access-denied";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});



builder.Services.AddScoped<IPurchasedProductRepository, PurchasedProductRepository>();
builder.Services.AddScoped<IPurchasedProductService, PurchasedProductService>();


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();


builder.Services.AddControllersWithViews();

// Use In-Memory database for development/testing
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseInMemoryDatabase("TestAuthDb"));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddScoped<IAuthService, AuthService>();



builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


builder.Services.AddScoped<IProductService>(sp =>
{
    var env = sp.GetRequiredService<IWebHostEnvironment>();
    var uploadPath = Path.Combine(env.WebRootPath, "uploads");
    var repo = sp.GetRequiredService<IProductRepository>();
    return new ProductService(repo, uploadPath);
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MarketSpot Auth API", Version = "v1" });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    await RoleSeeder.SeedRoles(roleManager);
    await RoleSeeder.SeedAdminUser(userManager);
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
