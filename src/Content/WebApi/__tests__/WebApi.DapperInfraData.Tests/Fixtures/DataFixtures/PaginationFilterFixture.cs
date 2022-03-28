using Bogus;
using WebApi.Domain.Repositories.Filters;

namespace WebApi.DapperInfraData.Tests.Fixtures.DataFixtures
{
    public static class PaginationFilterFixture
    {
        private static readonly Faker _faker = Fixture.Get();

        public static PaginationFilter Build() => new()
        {
            OrderBy = _faker.Lorem.Word(),
            PageNumber = _faker.Random.Int(min: 0),
            PageSize = _faker.Random.Int(min: 1, max: 255),
        };
    }
}
