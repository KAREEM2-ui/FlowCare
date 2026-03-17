using FlowCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowCare.Infrastructure.Persistence.Configurations;

public sealed class EntityTypeConfiguration : IEntityTypeConfiguration<EntityType>
{
    public void Configure(EntityTypeBuilder<EntityType> builder)
    {
        builder.ToTable("entity_types");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Type).HasMaxLength(100).IsRequired();
        builder.HasIndex(x => x.Type).IsUnique();
    }
}
