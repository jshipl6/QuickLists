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

        // POST: /Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title")] TaskItem input)
        {
            if (!ModelState.IsValid)
            {
                var items = await _tasks.GetAllAsync();
                return View("Index", items);
            }

            await _tasks.AddAsync(input.Title);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Tasks/Toggle/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Toggle(int id)
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
