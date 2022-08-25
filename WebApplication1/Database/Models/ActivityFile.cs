using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Database.Models
{
    public class ActivityFile
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Activity ActivityID { get; set; }
        [Required]
        public byte[] Data { get; set; }
        [Required]
        public string FileExtension { get; set; }
    }
}
