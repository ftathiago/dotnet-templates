using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Generic;
using WebApi.EfInfraData.Contexts;

namespace WebApi.EfInfraData.Tests.Fixtures
{
    public class DbContextFixture : WebApiDbContext
    {
        public DbContextFixture(DbContextOptions<WebApiDbContext> options)
            : base(options)
        {
        }

        public static DbContextFixture BuildContext()
        {
            var options = new DbContextOptions<WebApiDbContext>();
            var context = new DbContextFixture(options);
            context.Database.EnsureCreated();
            return context;
        }

        public void Seed<T>(IEnumerable<T> seed)
            where T : class
        {
            Set<T>().AddRange(seed);
            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseInMemoryDatabase(databaseName: "TestMemoryDatabase")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            base.OnConfiguring(optionsBuilder);
        }
    }
}
