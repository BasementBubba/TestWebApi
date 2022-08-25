using Microsoft.EntityFrameworkCore;
using WebApplication1.Database.Models;

namespace WebApplication1.Database
{
    public class BaseContext : DbContext
    {
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityFile> ActivityFiles { get; set; }

        public BaseContext(DbContextOptions<BaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
