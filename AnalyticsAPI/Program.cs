using AnalyticsAPI.Analytics;
using AnalyticsAPI.Analytics.Services;
using AnalyticsAPI.Analytics.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContextFactory
builder.Services.AddDbContextFactory<AnalyticsDbContext>(options =>
{
    // Assuming you're using SQLite. If not, adjust this line accordingly.
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register AnalyticsService as Singleton
builder.Services.AddSingleton<AnalyticsService>();

// Keep your existing AddSqliteDB if it's doing additional configuration
builder.Services.AddSqliteDB();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.MapFallbackToFile("index.html");
app.UseAuthorization();
app.MapControllers();

app.Run();