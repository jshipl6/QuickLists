using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuickLists.Data;
using QuickLists.Models;

namespace QuickLists.Services
{
    public sealed class TaskService : ITaskService
    {
        private readonly QuickListsContext _db;
        private readonly ILogger<TaskService> _logger;

        public TaskService(QuickListsContext db, ILogger<TaskService> logger)
        {
            _db = db;
            _logger = logger;
        }

        // READ   list
        public async Task<List<TaskItem>> GetAllAsync()
        {
            var items = await _db.Tasks
                .OrderBy(t => t.IsComplete)
                .ThenBy(t => t.Id)
                .ToListAsync();

            _logger.LogInformation(
                "Loaded task list, count {Count}",
                items.Count);

            return items;
        }

        // READ   single item
        public async Task<TaskItem?> GetAsync(int id)
        {
            var item = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (item == null)
            {
                _logger.LogWarning(
                    "GetAsync could not find task with id {TaskId}",
                    id);
            }
            else
            {
                _logger.LogInformation(
                    "Loaded task with id {TaskId} title {Title} isComplete {IsComplete}",
                    item.Id,
                    item.Title,
                    item.IsComplete);
            }

            return item;
        }

        // CREATE   main path for logging
        public async Task<bool> AddAsync(TaskItem task)
        {
            if (task == null)
            {
                _logger.LogWarning("AddAsync was called with a null task");
                return false;
            }

            task.Title = task.Title.Trim();

            if (string.IsNullOrWhiteSpace(task.Title))
            {
                _logger.LogWarning(
                    "AddAsync validation failed for empty title, isComplete {IsComplete}",
                    task.IsComplete);
                return false;
            }

            try
            {
                _db.Tasks.Add(task);
                await _db.SaveChangesAsync();

                // success log with structured fields
                _logger.LogInformation(
                    "Task created successfully, id {TaskId}, title {Title}, isComplete {IsComplete}",
                    task.Id,
                    task.Title,
                    task.IsComplete);

                return true;
            }
            catch (Exception ex)
            {
                // error log with exception and fields
                _logger.LogError(
                    ex,
                    "Error while creating task, title {Title}, isComplete {IsComplete}",
                    task.Title,
                    task.IsComplete);

                return false;
            }
        }

        // UPDATE   toggle complete
        public async Task<bool> ToggleCompleteAsync(int id)
        {
            var item = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (item == null)
            {
                _logger.LogWarning(
                    "ToggleCompleteAsync could not find task with id {TaskId}",
                    id);
                return false;
            }

            item.IsComplete = !item.IsComplete;
            await _db.SaveChangesAsync();

            _logger.LogInformation(
                "Toggled completion for task id {TaskId}, new isComplete {IsComplete}",
                item.Id,
                item.IsComplete);

            return true;
        }
        public async Task<List<TaskItem>> GetIncompleteTasksAsync()
        {
            return await _db.Tasks
                .FromSqlRaw("SELECT Id, Title, IsComplete FROM Tasks WHERE IsComplete = 0")
                .ToListAsync();
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (item == null)
            {
                _logger.LogWarning(
                    "DeleteAsync could not find task with id {TaskId}",
                    id);
                return false;
            }

            _db.Tasks.Remove(item);
            await _db.SaveChangesAsync();

            _logger.LogInformation(
                "Deleted task id {TaskId}, title {Title}",
                item.Id,
                item.Title);

            return true;
        }
    }
}
