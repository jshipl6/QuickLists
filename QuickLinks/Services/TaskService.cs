using Microsoft.EntityFrameworkCore;
using QuickLists.Data;           
using QuickLists.Models;

namespace QuickLists.Services
{
    public sealed class TaskService : ITaskService
    {
        private readonly QuickListsContext _db;

        public TaskService(QuickListsContext db)
        {
            _db = db;
        }

        public Task<List<TaskItem>> GetAllAsync() =>
            _db.TaskItems.OrderBy(t => t.IsComplete).ThenBy(t => t.Id).ToListAsync();

        public Task<TaskItem?> GetAsync(int id) =>
            _db.TaskItems.FirstOrDefaultAsync(t => t.Id == id);

        public async Task<TaskItem> AddAsync(string title)
        {
            var item = new TaskItem { Title = title.Trim(), IsComplete = false };
            _db.TaskItems.Add(item);
            await _db.SaveChangesAsync();
            return item;
        }

        public async Task<bool> ToggleCompleteAsync(int id)
        {
            var item = await _db.TaskItems.FirstOrDefaultAsync(t => t.Id == id);
            if (item is null) return false;

            item.IsComplete = !item.IsComplete;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _db.TaskItems.FirstOrDefaultAsync(t => t.Id == id);
            if (item is null) return false;

            _db.TaskItems.Remove(item);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
