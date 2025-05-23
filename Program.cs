using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using registrousuapi.Data;
using registrousuapi.Services; // Para PaisService

var builder = WebApplication.CreateBuilder(args);

// Leer cadena de conexión desde appsettings.json o variables de entorno
var conn = builder.Configuration.GetConnectionString("DefaultConnection") 
           ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Registrar DbContext con PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(conn));

// Registrar Identity con EF Core Stores
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Registrar controlador MVC y vistas
builder.Services.AddControllersWithViews();

// Registrar PaisService con HttpClient Factory para inyección
builder.Services.AddHttpClient<PaisService>();

var app = builder.Build();

// Pipeline HTTP
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
