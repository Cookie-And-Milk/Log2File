/* pre-requisite: 
execute the following commands to install necessary packages
- dotnet add package Serilog.AspNetCore
- dotnet add package Serilog.Extensions.Logging.File 

This is a ASP.NET Core MVC project. 
*/ 

using Serilog;

// create logger object
Log.Logger = new LoggerConfiguration()
    .WriteTo.File($"{Directory.GetCurrentDirectory()}/Logs/SeriLog.log", Serilog.Events.LogEventLevel.Debug)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(); // <-- u need this. No, really, it doesn't work without this. 

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // create an log entry if the environment is not development
    Log.Information("This is production environment! "); 
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    Log.CloseAndFlush(); 
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// create an log entry on start up
try
{
    Log.Information("App started. ");

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Application terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush(); 
}
