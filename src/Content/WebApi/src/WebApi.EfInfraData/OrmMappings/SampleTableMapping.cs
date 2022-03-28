using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.EfInfraData.Models;

namespace WebApi.EfInfraData.OrmMappings
{
    public class SampleTableMapping : IEntityTypeConfiguration<SampleTable>
    {
        public void Configure(EntityTypeBuilder<SampleTable> builder)
        {
            builder
                .ToTable(nameof(SampleTable));

            builder
                .Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder
                .Property(x => x.TestProperty)
                .HasColumnName("testProperty")
                .HasMaxLength(250)
                .IsRequired();

            builder
                .Property(x => x.Active)
                .HasColumnName("active")
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}
