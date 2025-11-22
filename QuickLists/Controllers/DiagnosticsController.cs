using Microsoft.AspNetCore.Mvc;
using QuickLists.Data;
using Microsoft.EntityFrameworkCore;

namespace QuickLists.Controllers
{
    public class DiagnosticsController : Controller
    {
        private readonly QuickListsContext _db;

        public DiagnosticsController(QuickListsContext db)
        {
            _db = db;
        }

        [HttpGet("/health")]
        public async Task<IActionResult> Health()
        {
            var health = new
            {
                status = "OK",
                databaseConnected = await CheckDatabase(),
                timestamp = DateTime.UtcNow
            };

            return Json(health);
        }

        private async Task<bool> CheckDatabase()
        {
            try
            {
                await _db.Database.ExecuteSqlRawAsync("SELECT 1");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
