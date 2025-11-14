using Microsoft.EntityFrameworkCore;
using QuickLists.Data;
using QuickLists.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickLists.Services
{
    public class TaskService : ITaskService
    {
        private readonly QuickListsContext _db;

        public TaskService(QuickListsContext db)
        {
            _db = db;
        }

        public async Task<List<TaskItem>> GetAllAsync()
        {
            return await _db.Tasks
                .OrderBy(t => t.IsComplete)
                .ThenBy(t => t.Id)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetAsync(int id)
        {
            return await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(TaskItem task)
        {
            // trim title just to be safe
            task.Title = task.Title?.Trim() ?? string.Empty;
            _db.Tasks.Add(task);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> ToggleCompleteAsync(int id)
        {
            var item = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (item == null) return false;

            item.IsComplete = !item.IsComplete;
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (item == null) return false;

            _db.Tasks.Remove(item);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
