using FlowCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowCare.Infrastructure.Persistence.Configurations;

public sealed class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.ToTable("staff");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Username).HasMaxLength(100).IsRequired();
        builder.Property(x => x.PasswordHash).HasMaxLength(500).IsRequired();


        builder.Property(x => x.FullName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(200).IsRequired();

        builder.Property(x => x.BranchId).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();

        builder.HasOne(x => x.Branch)
            .WithMany()
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.WorkingHours)
            .WithOne(x => x.Staff)
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ServiceTypes)
            .WithOne(x => x.Staff)
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Slots)
            .WithOne(x => x.Staff)
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Appointments)
            .WithOne(x => x.Staff)
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
