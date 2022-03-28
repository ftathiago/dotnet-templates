using WebApi.Domain.Entities;

namespace WebApi.Api.Models.Responses
{
    public class SampleResponse
    {
        public int Id { get; set; }

        public string TestProperty { get; set; }

        public bool Active { get; set; }

        public static SampleResponse From(SampleEntity sample) => new()
        {
            Id = sample.Id,
            TestProperty = sample.TestProperty,
            Active = sample.Active,
        };
    }
}
