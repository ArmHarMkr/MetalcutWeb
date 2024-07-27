using MetalcutWeb.DAL.Data;
using Microsoft.EntityFrameworkCore;
using MetalcutWeb.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using MetalcutWeb.Domain.Roles;
using MetalcutWeb.DAL.Repository;
using MetalcutWeb.DAL.Implementations;
using MetalcutWeb.Service.Interface;
using MetalcutWeb.Service.Implementation;
using MetalcutWeb.Service.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<AppUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IDeleteReferences<ProductEntity>, ProductRefDeleter>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

builder.Logging.AddConsole();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
IConfiguration configuration = app.Configuration;

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.MapRazorPages();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapAreaControllerRoute(
    name: "admin_area",
    areaName: "Admin",
    pattern: "admin/{controller=Home}/{action=Index}/{id?}"
);

app.MapAreaControllerRoute(
    name: "customers_area",
    areaName: "Customers",
    pattern: "Customers/{controller=Order}/{action=Index}/{id?}");



var logger = app.Services.GetService<ILogger<Program>>();
logger.LogInformation("Starting program...");


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { SD.Role_Employee, SD.Role_Customer, SD.Role_Manager, SD.Role_Admin };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    string email = configuration["AdminEmail"];
    string secondAdminEmail = configuration["SecondAdminEmail"];

    if (await userManager.FindByEmailAsync(email) != null)
    {
        var user = await userManager.FindByEmailAsync(email);
        await userManager.RemoveFromRoleAsync(user, SD.Role_Customer);
        await userManager.AddToRoleAsync(user, SD.Role_Admin);
    }

    if (await userManager.FindByEmailAsync(secondAdminEmail) != null)
    {
        var secondUserAdmin = await userManager.FindByEmailAsync(secondAdminEmail);
        await userManager.RemoveFromRoleAsync(secondUserAdmin, SD.Role_Customer);
        await userManager.AddToRoleAsync(secondUserAdmin, SD.Role_Admin);
    }
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        if (!await dbContext.Products.AnyAsync())
        {
            dbContext.Products.Add(new ProductEntity { Id = "PlasmaId", ProdName = "Plasma Cutting services", ProdDescription = "Send us your files via Email", Price = 200 });
            await dbContext.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex.Message, "Some error occured");
    }
}

app.Run();
