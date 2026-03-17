using FlowCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowCare.Infrastructure.Persistence.Configurations;

public sealed class StaffServiceTypeConfiguration : IEntityTypeConfiguration<StaffServiceType>
{
    public void Configure(EntityTypeBuilder<StaffServiceType> builder)
    {
        builder.ToTable("staff_service_types");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.StaffId).HasMaxLength(64).IsRequired();
        builder.Property(x => x.ServiceTypeId).HasMaxLength(64).IsRequired();

        builder.HasIndex(x => new { x.StaffId, x.ServiceTypeId }).IsUnique();

        builder.HasOne(x => x.Staff)
            .WithMany(x => x.ServiceTypes)
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ServiceType)
            .WithMany(x => x.Staff)
            .HasForeignKey(x => x.ServiceTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
