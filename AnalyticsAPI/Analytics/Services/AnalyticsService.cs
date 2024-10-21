using AnalyticsAPI.Analytics.Database;
using AnalyticsAPI.Analytics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AnalyticsAPI.Analytics.Services
{
    public class AnalyticsService
    {
        private readonly IDbContextFactory<AnalyticsDbContext> _contextFactory;
        private readonly ApiLoggingQueueService _loggingQueue;

        public AnalyticsService(IDbContextFactory<AnalyticsDbContext> contextFactory, int batchSize = 3, int maxBatchWaitMs = 300000)
        {
            _contextFactory = contextFactory;
            _loggingQueue = new ApiLoggingQueueService(batchSize, maxBatchWaitMs, ProcessBatchAsync);
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
            LogSuccessfulApiHit(logEntry);
        }

        private async Task ProcessBatchAsync(IEnumerable<RequestLog> batch)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            await context.RequestLogs.AddRangeAsync(batch);
            await context.SaveChangesAsync();
        }

        public void LogSuccessfulApiHit(RequestLog logEntry)
        {
            _loggingQueue.LogSuccessfulApiHit(logEntry);
        }

        public async Task<object> GetRequestLogs()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var requests = await context.RequestLogs.ToListAsync();
            return requests;
        }
    }
}