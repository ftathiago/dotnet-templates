using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApi.EfInfraData.Models;

namespace WebApi.EfInfraData.Contexts
{
    public class WebApiDbContext : DbContext
    {
        public WebApiDbContext(DbContextOptions<WebApiDbContext> options)
            : base(options)
        {
        }

        protected WebApiDbContext()
        {
            // Just for mocking test
        }

        public virtual DbSet<SampleTable> SampleTables { get; init; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
