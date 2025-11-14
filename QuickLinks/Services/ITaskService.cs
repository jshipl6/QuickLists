using QuickLists.Models;

namespace QuickLists.Services
{
    public interface ITaskService
    {
        Task<List<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetAsync(int id);
        Task<TaskItem> AddAsync(string title);
        Task<bool> ToggleCompleteAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
