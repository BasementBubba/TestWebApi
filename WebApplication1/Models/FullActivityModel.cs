namespace WebApplication1.Models
{
    public class FullActivityModel
    {
        public ActivityModel Activity { get; set; }
        public IEnumerable<ActivityFileModel> Files { get; set; }
    }
}
