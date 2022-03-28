namespace WebApi.EfInfraData.Models
{
    public record SampleTable
    {
        public int Id { get; set; }

        public string TestProperty { get; set; }

        public bool Active { get; set; }
    }
}
