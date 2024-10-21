using AnalyticsAPI.Analytics.Models;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsAPI.Analytics.Database
{
    public class AnalyticsDbContext:DbContext
    {
        public DbSet<RequestLog> RequestLogs { get; set; }

        public AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=analytics.db");
        
        }
    }
}
