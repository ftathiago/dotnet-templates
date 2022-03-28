using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Moq;
using System;
using System.Net;
using WebApi.Api.Controllers;
using WebApi.Api.Localization;
using WebApi.Api.Models.Requests;
using WebApi.Api.Tests.Fixtures;
using WebApi.Domain.Entities;
using WebApi.Domain.Repositories;
using WebApi.Domain.Services;
using WebApi.Shared.Extensions;
using Xunit;

namespace WebApi.Api.Tests.Controllers
{
    public class SampleControllerTests
    {
        private readonly Faker _faker = Fixture.Get();
        private readonly Mock<ISampleService> _service;
        private readonly Mock<ISampleRepository> _repository;
        private readonly Mock<IStringLocalizer<Resource>> _stringLocalizer;

        public SampleControllerTests()
        {
            _service = new Mock<ISampleService>(MockBehavior.Strict);
            _repository = new Mock<ISampleRepository>(MockBehavior.Strict);
            _stringLocalizer = new Mock<IStringLocalizer<Resource>>(MockBehavior.Strict);
        }

        [Fact]
        public void Should_ReturnOkResultWithSampleResponse()
        {
            // Given
            var mockSampleTable = GetSampleEntity();
            _service
                .Setup(s => s.GetSampleBy(mockSampleTable.Id))
                .Returns(mockSampleTable);
            var controller = GetController();

            // When
            var response = controller.Get(mockSampleTable.Id);
            var result = response as OkObjectResult;

            // Then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(HttpStatusCode.OK.AsInteger());
        }

        [Fact]
        public void Should_ReturnBadRequest_When_IdIsNull()
        {
            // Given
            var controller = GetController();

            // When
            var response = controller.Get(null);
            var result = response as BadRequestResult;

            // Then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest.AsInteger());
        }

        [Fact]
        public void Should_ReturnNotFound_When_SampleDoesNotExists()
        {
            // Given
            var notExistId = Fixture.Get().Random.Int();
            _service
                .Setup(s => s.GetSampleBy(It.IsAny<int>()))
                .Returns(null as SampleEntity);
            var controller = GetController();

            // When
            var response = controller.Get(notExistId);
            var result = response as NotFoundResult;

            // Then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound.AsInteger());
        }

        [Fact]
        public void Should_EchoQueryParams()
        {
            // Given
            var queryString = new QueryStringSample
            {
                Id = Guid.NewGuid(),
                QueryNumber = _faker.Random.Int(),
                RequiredField = _faker.Lorem.Sentences(),
            };
            var page = new PaginationRequest
            {
                PageNumber = _faker.Random.Int(PaginationRequest.MinOffset),
                RecordPerPage = _faker.Random.Int(PaginationRequest.MaxRecordsPerPage),
            };
            var controller = GetController();

            // When
            var result = controller.GetObjectFromQuery(queryString, page);

            // Then
            result.Should().BeOfType<OkObjectResult>();
            var objectResponse = (result as OkObjectResult)?.Value;
            objectResponse.Should().BeEquivalentTo(new
            {
                queryString,
                page,
            });
        }

        private static SampleEntity GetSampleEntity() => new()
        {
            Id = Fixture.Get().Random.Int(),
            TestProperty = Fixture.Get().Lorem.Sentence(),
            Active = Fixture.Get().Random.Bool(),
        };

        private SampleController GetController() => new(
                _service.Object,
                _repository.Object,
                _stringLocalizer.Object);
    }
}
