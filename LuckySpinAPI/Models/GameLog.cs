using System;

namespace LuckySpinAPI.Models
{
    public class GameLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal BetAmount { get; set; }
        public decimal WinAmount { get; set; }
        public string Combination { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
