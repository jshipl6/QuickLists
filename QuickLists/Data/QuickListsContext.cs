using Microsoft.EntityFrameworkCore;
using QuickLists.Models;

namespace QuickLists.Data
{
    public class QuickListsContext : DbContext
    {
        public QuickListsContext(DbContextOptions<QuickListsContext> options)
            : base(options)
        {
        }

        public DbSet<TaskItem> Tasks { get; set; } = default!;
    }
}
