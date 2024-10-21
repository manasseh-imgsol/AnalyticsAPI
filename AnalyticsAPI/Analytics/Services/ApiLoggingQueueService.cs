namespace AnalyticsAPI.Analytics.Services
{
    using AnalyticsAPI.Analytics.Models;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class ApiLoggingQueueService
    {
        private readonly ConcurrentQueue<RequestLog> _queue = new ConcurrentQueue<RequestLog>();
        private readonly int _batchSize;
        private readonly int _maxBatchWaitMs;
        private readonly Func<IEnumerable<RequestLog>, Task> _processBatchAsync;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public ApiLoggingQueueService(int batchSize, int maxBatchWaitMs, Func<IEnumerable<RequestLog>, Task> processBatchAsync)
        {
            _batchSize = batchSize;
            _maxBatchWaitMs = maxBatchWaitMs;
            _processBatchAsync = processBatchAsync;

            Task.Run(ProcessQueueAsync);
        }

        public void LogSuccessfulApiHit(RequestLog logEntry)
        {
            _queue.Enqueue(logEntry);
        }

        private async Task ProcessQueueAsync()
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                var batch = new List<RequestLog>();
                var dequeueTimeout = DateTime.UtcNow.AddMilliseconds(_maxBatchWaitMs);

                while (batch.Count < _batchSize && DateTime.UtcNow < dequeueTimeout)
                {
                    if (_queue.TryDequeue(out var item))
                    {
                        batch.Add(item);
                    }
                    else
                    {
                        await Task.Delay(10, _cts.Token); // Short delay to prevent tight loop
                    }
                }

                if (batch.Count > 0)
                {
                    try
                    {
                        await _processBatchAsync(batch);
                    }
                    catch (Exception ex)
                    {
                        // Log the exception or handle it as appropriate for your application
                        Console.WriteLine($"Error processing batch: {ex}");
                    }
                }
            }
        }

        public void Stop()
        {
            _cts.Cancel();
        }
    }

}
