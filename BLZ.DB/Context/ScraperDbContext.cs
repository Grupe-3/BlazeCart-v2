using Microsoft.EntityFrameworkCore;
using BLZ.Common.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BLZ.DB.Context
{
    public class ScraperDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Report> Reports { get; set; }

        public ScraperDbContext(DbContextOptions<ScraperDbContext> options) : base(options) { }

        /* Note: Since items depend on categories, they must be added first. */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

