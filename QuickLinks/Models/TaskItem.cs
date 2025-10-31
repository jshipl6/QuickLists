using System.ComponentModel.DataAnnotations;

namespace QuickLists.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public bool IsComplete { get; set; }
    }
}
