using FlowCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowCare.Infrastructure.Configurations
{
    public class BranchPositionConfig : IEntityTypeConfiguration<BranchSlotPosition>
    {
        public void Configure(EntityTypeBuilder<BranchSlotPosition> builder)
        {
            builder.HasNoKey().ToView(null);
        }
    }
}
