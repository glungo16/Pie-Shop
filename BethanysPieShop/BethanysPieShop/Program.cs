using BethanysPieShop.App;
using BethanysPieShop.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

// when builder is created, some defaults are also applied, including appsettings.json
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BethanysPieShopIdentityDbContextConnection") ?? throw new InvalidOperationException("Connection string 'BethanysPieShopIdentityDbContextConnection' not found.");

builder.Services.AddDbContext<BethanysPieShopDbContext>(options =>
{
	options.UseSqlServer(connectionString);
});

builder.Services.AddDefaultIdentity<IdentityUser>()
	.AddEntityFrameworkStores<BethanysPieShopDbContext>();

//builder.Services.AddDbContext<BethanysPieShopDbContext>(options =>
//{
//	options.UseSqlServer(
//		builder.Configuration["ConnectionStrings:BethanysPieShopDbContextConnection"]);
//});

//AddScoped = a singleton per http request
// every time we want to implement an object of type ICategoryRepository, we implement a MockCategoryRepository (it is injected)
// this is loose coupling (instead of tight coupling)
//builder.Services.AddScoped<ICategoryRepository, MockCategoryRepository>();
//builder.Services.AddScoped<IPieRepository, MockPieRepository>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPieRepository, PieRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();


builder.Services.AddScoped<IShoppingCart, ShoppingCart>(sp => ShoppingCart.GetCart(sp));
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();


// app knows about mvc; 
//builder.Services.AddControllers(); // this is needed for APIs; however, since we already have controllers with views, no need for it
builder.Services.AddControllersWithViews()
	.AddJsonOptions(options =>
	{
        // used to avoid cycles in json (e.g.: Pie -> Category -> Pie...)
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; 
	});

// app knows about razor pages (routing to be done with razor pages) - turns pages into actions - no need for controllers
builder.Services.AddRazorPages();

// adds support for blazor with server-side rendering
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents(); // server-side rendering



var app = builder.Build();

// middleware

//app.MapGet("/", () => "Hello World!");


// app knows about the wwwroot folder
app.UseStaticFiles();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage(); 
}

// makes the app know how to route to pages we are going to have
// it allows us to see our pages
app.MapDefaultControllerRoute(); // "{controller=Home}/{action=Index}/{id?}"

// this is equivalent to the above default
//app.MapControllerRoute(
//	name: "default",
//	pattern: "{controller=Home}/{action=Index}/{id?}");


app.UseAntiforgery(); // needed for blazor to work (antiforgery protection)

app.MapRazorPages();

// this is needed for APIs; however, since we already have MapDefaultControllerRoute, no need for it
//app.MapControllers();

// recognizes blazor components to be added in code
app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

DbInitializer.Seed(app);

app.Run();