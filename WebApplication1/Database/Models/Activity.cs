using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Database.Models
{
    public class Activity
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsCompleted { get; set; } = false;
        [Required]
        public DateTime CreationDate { get; set; }
        public DateTime? CompleteTime { get; set; }
    }
}
