using Microsoft.EntityFrameworkCore;
using QuickLists.Models;
using System.Collections.Generic;

namespace QuickLists.Data
{
    public class QuickListsContext : DbContext
    {
        public QuickListsContext(DbContextOptions<QuickListsContext> options)
            : base(options)
        {
        }

        public DbSet<TaskItem> TaskItems { get; set; }
    }
}
