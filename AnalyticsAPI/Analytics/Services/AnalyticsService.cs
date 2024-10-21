using AnalyticsAPI.Analytics.Database;
using AnalyticsAPI.Analytics.Models;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsAPI.Analytics.Services
{
    public class AnalyticsService
    {
        private readonly AnalyticsDbContext _context;
        private readonly ApiLoggingQueueService _loggingQueue;

        public AnalyticsService(AnalyticsDbContext context, int batchSize = 3, int maxBatchWaitMs = 5000)
        {
            _context = context;
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

            //_context.RequestLogs.Add(logEntry);
            //await _context.SaveChangesAsync();

            LogSuccessfulApiHit(logEntry);
        }

        // Batch save for request logs
        private async Task ProcessBatchAsync(IEnumerable<RequestLog> batch)
        {
            await _context.RequestLogs.AddRangeAsync(batch);
            await _context.SaveChangesAsync();
        }

        public void LogSuccessfulApiHit(RequestLog logEntry)
        {
            // Enqueue the log entry to the logging queue
            _loggingQueue.LogSuccessfulApiHit(logEntry);
        }

        public async Task<object> GetRequestLogs()
        {
            var requests = await _context.RequestLogs.ToListAsync();
            return requests;
        }
    }
}
