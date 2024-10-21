using System.ComponentModel.DataAnnotations;

namespace AnalyticsAPI.Analytics.Models
{
    public class RequestLog
    {
        [Key]
        public Guid Id { get; set; }
        public string Endpoint { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public bool IsSuccess { get; set; }
        public int TokensUsed { get; set; }
    }
}
