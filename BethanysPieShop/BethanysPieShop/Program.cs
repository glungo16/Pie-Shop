using BethanysPieShop.Models;

var builder = WebApplication.CreateBuilder(args);


//AddScoped = a singleton per http request
// every time we want to implement an object of type ICategoryRepository, we implement a MockCategoryRepository (it is injected)
// this is loose coupling (instead of tight coupling)
builder.Services.AddScoped<ICategoryRepository, MockCategoryRepository>();
builder.Services.AddScoped<IPieRepository, MockPieRepository>();

// app knows about mvc
builder.Services.AddControllersWithViews();


var app = builder.Build();

// middleware

//app.MapGet("/", () => "Hello World!");


// app knows about the wwwroot folder
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage(); 
}

// makes the app know how to route to pages we are going to have
// it allows us to see our pages
app.MapDefaultControllerRoute();


app.Run();