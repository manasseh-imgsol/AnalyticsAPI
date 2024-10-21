using AnalyticsAPI.Analytics.Database;
using AnalyticsAPI.Analytics.Models;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsAPI.Analytics.Services
{
    public class AnalyticsService
    {
        private readonly AnalyticsDbContext _context;

        public AnalyticsService(AnalyticsDbContext context)
        {
            _context = context;
        }

        public async Task LogRequestAsync(string endpoint, bool isSuccess, int tokensUsed)
        {
            var logEntry = new RequestLog
            {
                Id = Guid.NewGuid(),
                Endpoint = endpoint,
                Timestamp = DateTime.UtcNow,
                IsSuccess = isSuccess,
                TokensUsed = tokensUsed
            };

            _context.RequestLogs.Add(logEntry);
            await _context.SaveChangesAsync();
        }

        public async Task<object> GetRequestLogs()
        {
            var requests = await _context.RequestLogs.ToListAsync();
            return requests;
        }
    }
}
