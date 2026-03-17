using FlowCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowCare.Infrastructure.Persistence.Configurations;

public sealed class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("branches");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.City).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Address).HasMaxLength(300).IsRequired();
        builder.Property(x => x.Timezone).HasMaxLength(64).IsRequired();

        builder.Property(x => x.IsActive).IsRequired();

        builder.HasMany(x => x.WorkingHours)
            .WithOne(x => x.Branch)
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ServiceTypes)
            .WithOne(x => x.Branch)
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Slots)
            .WithOne(x => x.Branch)
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
