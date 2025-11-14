using Microsoft.AspNetCore.Mvc;
using QuickLists.Models;
using QuickLists.Services;

namespace QuickLists.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskService _tasks;

        public TasksController(ITaskService tasks)
        {
            _tasks = tasks;
        }

        // GET: /Tasks
        public async Task<IActionResult> Index()
        {
            var items = await _tasks.GetAllAsync();
            return View(items);
        }

        // GET: /Tasks/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new TaskItem());
        }

        // POST: /Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,IsComplete")] TaskItem task)
        {
            if (!ModelState.IsValid)
            {
                return View(task);
            }

            await _tasks.AddAsync(task);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Tasks/ToggleComplete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleComplete(int id)
        {
            await _tasks.ToggleCompleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Tasks/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _tasks.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
