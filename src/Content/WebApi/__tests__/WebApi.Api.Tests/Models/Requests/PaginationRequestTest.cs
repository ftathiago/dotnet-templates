using Bogus;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using WebApi.Api.Models.Requests;
using WebApi.Api.Tests.Fixtures;
using Xunit;

namespace WebApi.Api.Tests.Models.Requests
{
    public class PaginationRequestTest
    {
        private readonly Faker _faker = Fixture.Get();

        [Fact]
        public void Should_BeValid_When_RangesAreOk()
        {
            // Given
            var pagination = new PaginationRequest()
            {
                PageNumber = _faker.Random.Number(
                    min: PaginationRequest.MinOffset,
                    max: int.MaxValue),
                RecordPerPage = _faker.Random.Number(
                    min: PaginationRequest.MinRecordsPerPage,
                    max: PaginationRequest.MaxRecordsPerPage),
            };
            var context = new ValidationContext(pagination);

            // When
            var validation = pagination.Validate(context);

            // Then
            validation.Should().BeEmpty();
        }

        [Fact]
        public void Should_BeInvalid_When_OffsetIsLessThanOne()
        {
            // Given
            const string ExpectedMessage = "Parameter _page must be greather or equal than 1";
            var pagination = new PaginationRequest()
            {
                PageNumber = 0,
            };
            var context = new ValidationContext(pagination);

            // When
            var validation = pagination.Validate(context);

            // Then
            validation.Should().HaveCount(1);
            validation.Should().Contain(x => x.ErrorMessage == ExpectedMessage);
        }

        [Fact]
        public void Should_BeInvalid_When_ViolatesMaxRecordsPerPage()
        {
            // Given
            const string ExpectedMessage =
                "Parameter _size must be specified between 1 and 50.";
            var pagination = new PaginationRequest()
            {
                RecordPerPage = PaginationRequest.MaxRecordsPerPage + 1,
            };
            var context = new ValidationContext(pagination);

            // When
            var validation = pagination.Validate(context);

            // Then
            validation.Should().HaveCount(1);
            validation.Should().Contain(x => x.ErrorMessage == ExpectedMessage);
        }
    }
}
