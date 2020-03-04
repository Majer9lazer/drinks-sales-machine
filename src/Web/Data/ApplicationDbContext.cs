using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Coin> Coins { get; set; }
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Machine> Machines { get; set; }


    }
}
