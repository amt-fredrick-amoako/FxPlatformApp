using FxPlatformApp;
using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
//services
builder.Services.AddControllersWithViews();
//builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));
builder.Services.AddSingleton<IStocksService, StocksService>();
builder.Services.AddSingleton<IFinnhubService, FinnhubService>();
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection(nameof(TradingOptions)));
builder.Services.AddHttpClient();

var app = builder.Build();
//middlewares
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
