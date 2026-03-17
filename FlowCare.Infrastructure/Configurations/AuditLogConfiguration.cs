using FlowCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowCare.Infrastructure.Persistence.Configurations;

public sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("audit_logs");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.ActorId).IsRequired();


        builder.Property(x => x.EntityId).IsRequired();
        builder.HasOne(x => x.ActionType).WithMany().HasForeignKey(x => x.ActionTypeId);
        builder.HasOne(x => x.ActionType).WithMany().HasForeignKey(x => x.ActionTypeId);
        builder.HasOne(x=> x.Role).WithMany().HasForeignKey(x => x.RoleId);

        builder.Property(x => x.Timestamp).IsRequired();
        builder.Property(x => x.MetadataJson).HasColumnType("jsonb");
    }
}
