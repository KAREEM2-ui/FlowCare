using FlowCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowCare.Infrastructure.Persistence.Configurations;

public sealed class ActionTypeConfiguration : IEntityTypeConfiguration<ActionType>
{
    public void Configure(EntityTypeBuilder<ActionType> builder)
    {
        builder.ToTable("action_types");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Type).HasMaxLength(100).IsRequired();
        builder.HasIndex(x => x.Type).IsUnique();
    }
}
