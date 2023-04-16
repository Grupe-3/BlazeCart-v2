using Microsoft.EntityFrameworkCore.Design;

using Context = BLZ.DB.Context.ScraperDbContext;
namespace BLZ.Functions.DesignTimeDB
{
    public class ScraperContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            return new Context(DbJsonExtension<Context>.GetOptions());
        }
    }
}
