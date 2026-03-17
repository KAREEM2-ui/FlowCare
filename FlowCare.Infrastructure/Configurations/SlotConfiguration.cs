using FlowCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowCare.Infrastructure.Persistence.Configurations;

public sealed class SlotConfiguration : IEntityTypeConfiguration<Slot>
{
    public void Configure(EntityTypeBuilder<Slot> builder)
    {
        builder.ToTable("slots");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.BranchId).IsRequired();
        builder.Property(x => x.ServiceTypeId).IsRequired();
        builder.Property(x => x.StaffId).IsRequired();

        builder.Property(x => x.StartAt).IsRequired();
        builder.Property(x => x.EndAt).IsRequired();

        builder.Property(x => x.Capacity).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();

        builder.HasOne(x => x.Branch)
            .WithMany(x => x.Slots)
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ServiceType)
            .WithMany(x => x.Slots)
            .HasForeignKey(x => x.ServiceTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Staff)
            .WithMany(x => x.Slots)
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Appointments)
            .WithOne(x => x.Slot)
            .HasForeignKey(x => x.SlotId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.BranchId, x.ServiceTypeId, x.StaffId, x.StartAt }).IsUnique(false);
    }
}
