using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowCare.Application.DTOs.Responses
{
    public sealed record BranchSlotPositionResponse(
    string? FullName,
    DateTimeOffset StartAt,
    DateTimeOffset EndAt,
    string? Service,
    long? RankPosition);
}
