using BethanysPieShop.Models;
using Microsoft.EntityFrameworkCore;

// when builder is created, some defaults are also applied, including appsettings.json
var builder = WebApplication.CreateBuilder(args);


//AddScoped = a singleton per http request
// every time we want to implement an object of type ICategoryRepository, we implement a MockCategoryRepository (it is injected)
// this is loose coupling (instead of tight coupling)
//builder.Services.AddScoped<ICategoryRepository, MockCategoryRepository>();
//builder.Services.AddScoped<IPieRepository, MockPieRepository>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPieRepository, PieRepository>();


builder.Services.AddScoped<IShoppingCart, ShoppingCart>(sp => ShoppingCart.GetCart(sp));
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();


// app knows about mvc
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BethanysPieShopDbContext>(options =>
{
	options.UseSqlServer(
		builder.Configuration["ConnectionStrings:BethanysPieShopDbContextConnection"]);
});


var app = builder.Build();

// middleware

//app.MapGet("/", () => "Hello World!");


// app knows about the wwwroot folder
app.UseStaticFiles();

app.UseSession();

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

DbInitializer.Seed(app);

app.Run();