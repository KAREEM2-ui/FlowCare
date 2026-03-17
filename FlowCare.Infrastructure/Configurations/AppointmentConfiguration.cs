using FlowCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowCare.Infrastructure.Persistence.Configurations;

public sealed class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("appointments");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.CustomerId).IsRequired();
        builder.Property(x => x.BranchId).IsRequired();
        builder.Property(x => x.ServiceTypeId).IsRequired();
        builder.Property(x => x.SlotId).IsRequired();
        builder.Property(x => x.StaffId).IsRequired();

        builder.Property(x => x.Status).HasConversion<int>().IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasOne(x => x.Branch)
            .WithMany()
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ServiceType)
            .WithMany()
            .HasForeignKey(x => x.ServiceTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Staff)
            .WithMany(x => x.Appointments)
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Slot)
            .WithMany(x => x.Appointments)
            .HasForeignKey(x => x.SlotId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.SlotId).IsUnique(true);
    }
}
