using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<UserInformation> UserInformation { get; set; }
        public DbSet<HumanInformation> HumanInformation { get; set; }
        public DbSet<LocationInformation> LocationInformation { get; set; }
    }
}
