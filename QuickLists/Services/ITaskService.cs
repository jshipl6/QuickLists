using System.Collections.Generic;
using System.Threading.Tasks;
using QuickLists.Models;

namespace QuickLists.Services
{
    public interface ITaskService
    {
        Task<List<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetAsync(int id);
        Task<bool> AddAsync(TaskItem task);
        Task<bool> ToggleCompleteAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
