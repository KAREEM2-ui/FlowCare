using FlowCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowCare.Infrastructure.Persistence.Configurations;

public sealed class BranchWorkingHourConfiguration : IEntityTypeConfiguration<BranchWorkingHour>
{
    public void Configure(EntityTypeBuilder<BranchWorkingHour> builder)
    {
        builder.ToTable("branch_working_hours");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BranchId).HasMaxLength(64).IsRequired();

        builder.Property(x => x.DayOfWeek).HasConversion<int>().IsRequired();
        builder.Property(x => x.StartTime).IsRequired();
        builder.Property(x => x.EndTime).IsRequired();

        builder.HasIndex(x => new { x.BranchId, x.DayOfWeek }).IsUnique(true);
    }
}
