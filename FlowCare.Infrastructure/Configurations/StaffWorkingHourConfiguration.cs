using FlowCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowCare.Infrastructure.Persistence.Configurations;

public sealed class StaffWorkingHourConfiguration : IEntityTypeConfiguration<StaffWorkingHour>
{
    public void Configure(EntityTypeBuilder<StaffWorkingHour> builder)
    {
        builder.ToTable("staff_working_hours");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.StaffId).HasMaxLength(64).IsRequired();

        builder.Property(x => x.DayOfWeek).HasConversion<int>().IsRequired();
        builder.Property(x => x.StartTime).IsRequired();
        builder.Property(x => x.EndTime).IsRequired();

        builder.HasIndex(x => new { x.StaffId, x.DayOfWeek }).IsUnique(false);
    }
}
