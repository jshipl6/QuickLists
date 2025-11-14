using QuickLists.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickLists.Services
{
    public interface ITaskService
    {
        Task<List<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetAsync(int id);
        Task<bool> ToggleCompleteAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task AddAsync(TaskItem task);
    }
}
