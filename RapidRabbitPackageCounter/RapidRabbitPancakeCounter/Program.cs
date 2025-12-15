using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RapidRabbitPancakeCounter.Data;

using static RapidRabbitPancakeCounter.Common.Constants.LogMessages;

var builder = WebApplication.CreateBuilder(args);

// Get connection string from environment variables or fallback to appsettings.json
var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION") 
                       ?? builder.Configuration.GetConnectionString("DefaultConnection");

// If the connection sting is empty app should not be started.
if (string.IsNullOrEmpty(connectionString))
    throw new ArgumentNullException(ConnectionStringIsMissing);

// Add DbContext with PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add services to the container.
// Add MediatR
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Auto-migrate database on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    try
    {
        dbContext.Database.Migrate();
        app.Logger.LogInformation(DatabaseMigrationCompletedSuccessfully);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, DatabaseMigrationErrorMsg);
        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();