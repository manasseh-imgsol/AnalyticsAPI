using AnalyticsAPI.Analytics.Database;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsAPI.Analytics
{
    public static class AnalyticsServiceExtensions
    {
        public static void AddSqliteDB(this IServiceCollection services)
        {
            services.AddDbContext<AnalyticsDbContext>(options =>
                options.UseSqlite("analytics.db"));
        }

    }
}
