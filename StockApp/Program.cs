using ServiceContracts;
using Services;
using StockApp;

var builder = WebApplication.CreateBuilder(args);

//Services

builder.Services.AddControllersWithViews();
//builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));
builder.Services.AddTransient<IFinnhubServices, FinnhubServices>();
builder.Services.AddHttpClient();


var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();