using FlowCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowCare.Infrastructure.Persistence.Configurations;

public sealed class ServiceTypeConfiguration : IEntityTypeConfiguration<ServiceType>
{
    public void Configure(EntityTypeBuilder<ServiceType> builder)
    {
        builder.ToTable("service_types");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.BranchId).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.DurationMinutes).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();

        builder.HasOne(x => x.Branch)
            .WithMany(x => x.ServiceTypes)
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Slots)
            .WithOne(x => x.ServiceType)
            .HasForeignKey(x => x.ServiceTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
