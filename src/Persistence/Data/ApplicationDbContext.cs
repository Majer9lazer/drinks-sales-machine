using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using Persistence.Utils;

namespace Persistence.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Coin> Coins { get; set; }
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<MachineCoin> MachineCoins { get; set; }
        public DbSet<MachineDrink> MachineDrinks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Image>().HasData(ApplicationDbDefaultValuesContainer.GetDefaultImages());
            builder.Entity<Coin>().HasData(ApplicationDbDefaultValuesContainer.GetDefaultCoins());
            builder.Entity<Drink>().HasData(ApplicationDbDefaultValuesContainer.GetDefaultDrinks());
            base.OnModelCreating(builder);
        }
    }
}
