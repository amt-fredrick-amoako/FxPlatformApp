var builder = WebApplication.CreateBuilder(args);
//services
builder.Services.AddControllersWithViews();

var app = builder.Build();
//middlewares
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
