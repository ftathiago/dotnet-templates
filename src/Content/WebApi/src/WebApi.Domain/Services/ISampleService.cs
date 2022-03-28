using WebApi.Domain.Entities;

namespace WebApi.Domain.Services
{
    public interface ISampleService
    {
        SampleEntity GetSampleBy(int id);
    }
}
