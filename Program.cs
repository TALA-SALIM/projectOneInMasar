using homework.Data;
using homework.Enums;
using homework.Models;
using homework.Services;
using homework.Services.email;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddIdentity<User, IdentityRole>(options =>
        options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IFileService, FileService>();

var app = builder.Build();

await InitializeRolesAndAdminAsync(app);

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
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
    .WithStaticAssets();

app.Run();

static async Task InitializeRolesAndAdminAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var roleManager =
        services.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new List<string> { "Admin", "Employee" };

    foreach (var roleName in roles)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var result =
                await roleManager.CreateAsync(new IdentityRole(roleName));

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(error => error.Description));

                throw new InvalidOperationException(
                    $"Could not create role {roleName}: {errors}");
            }
        }
    }

    var adminEmail = app.Configuration["AdminSeed:Email"];
    var adminPassword = app.Configuration["AdminSeed:Password"];

    // إذا لم تضعي بيانات Admin، سيُنشئ البرنامج الأدوار فقط.
    if (string.IsNullOrWhiteSpace(adminEmail) ||
        string.IsNullOrWhiteSpace(adminPassword))
    {
        return;
    }

    var userManager = services.GetRequiredService<UserManager<User>>();
    var admin = await userManager.FindByEmailAsync(adminEmail);

    if (admin is null)
    {
        admin = new User
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            UserType = UserType.Admin,
            CreatedAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(admin, adminPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(
                ", ",
                result.Errors.Select(error => error.Description));

            throw new InvalidOperationException(
                $"Could not create Admin account: {errors}");
        }
    }

    if (!await userManager.IsInRoleAsync(admin, "Admin"))
    {
        var result = await userManager.AddToRoleAsync(admin, "Admin");

        if (!result.Succeeded)
        {
            var errors = string.Join(
                ", ",
                result.Errors.Select(error => error.Description));

            throw new InvalidOperationException(
                $"Could not assign Admin role: {errors}");
        }
    }
}