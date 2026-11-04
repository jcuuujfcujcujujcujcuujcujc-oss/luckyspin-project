using System;

namespace LuckySpinAPI.Services
{
    public class SlotEngine
    {
        private readonly string[] _symbols = { "🍒", "🍋", "🍇", "💎", "7️⃣" };
        private readonly Random _random = new Random();

        public (string Combination, decimal WinMultiplier) Spin()
        {
            var reel1 = _symbols[_random.Next(_symbols.Length)];
            var reel2 = _symbols[_random.Next(_symbols.Length)];
            var reel3 = _symbols[_random.Next(_symbols.Length)];

            string combination = $"{reel1}|{reel2}|{reel3}";
            decimal multiplier = 0;

            if (reel1 == reel2 && reel2 == reel3)
            {
                multiplier = reel1 switch
                {
                    "7️⃣" => 50,
                    "💎" => 20,
                    _ => 10
                };
            }
            else if (reel1 == reel2 || reel2 == reel3)
            {
                multiplier = 2;
            }

            return (combination, multiplier);
        }
    }
}
