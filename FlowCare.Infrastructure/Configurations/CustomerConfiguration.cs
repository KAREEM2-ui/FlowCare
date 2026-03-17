using FlowCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowCare.Infrastructure.Persistence.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Username).HasMaxLength(100).IsRequired();

        builder.Property(x => x.PasswordHash).HasMaxLength(500).IsRequired();

        builder.Property(x => x.FullName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Phone).HasMaxLength(50);
        builder.Property(x => x.IsActive).IsRequired();

        builder.HasMany(x => x.Appointments)
            .WithOne(x => x.Customer)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
