using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LuckySpinAPI.Data;
using LuckySpinAPI.Models;
using System;
using System.Threading.Tasks;

namespace LuckySpinAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SlotsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SlotsController(AppDbContext context)
        {
            _context = context;
        }

        // Эндпоинт загрузки баланса: GET /api/slots/balance/1
        [HttpGet("balance/{userId}")]
        public async Task<IActionResult> GetBalance(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            // Если In-Memory БД чистая и пользователя нет — создаем его автоматически
            if (user == null)
            {
                user = new User { Id = userId, Balance = 1000 };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            return Ok(new { balance = user.Balance });
        }

        // Эндпоинт совершения ставки: POST /api/slots/spin
        [HttpPost("spin")]
        public async Task<IActionResult> Spin([FromBody] SpinRequest request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return NotFound("Пользователь не найден. Нужна регистрация.");
            }

            if (user.Balance < request.BetAmount)
            {
                return BadRequest("Недостаточно кредитов на балансе!");
            }

            // Списываем ставку
            user.Balance -= request.BetAmount;

            // Логика генерации символов
            string[] symbols = { "🍒", "🍋", "777" };
            Random rand = new Random();
            string s1 = symbols[rand.Next(symbols.Length)];
            string s2 = symbols[rand.Next(symbols.Length)];
            string s3 = symbols[rand.Next(symbols.Length)];

            string combination = $"{s1}|{s2}|{s3}";
            bool isWin = (s1 == s2 && s2 == s3);
            decimal winAmount = 0;

            if (isWin)
            {
                winAmount = request.BetAmount * 5; // Выигрыш x5
                user.Balance += winAmount;
            }

            // Сохраняем новое состояние баланса в базу данных
            await _context.SaveChangesAsync();

            return Ok(new
            {
                combination = combination,
                newBalance = user.Balance,
                isWin = isWin,
                winAmount = winAmount
            });
        }
    }

    // Класс-модель для входящего JSON-запроса ставки
    public class SpinRequest
    {
        public int UserId { get; set; }
        public decimal BetAmount { get; set; }
    }
}
