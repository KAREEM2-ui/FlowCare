using FlowCare.Domain.Enums;
using FlowCare_presentation.validations;
using Microsoft.AspNetCore.Http;

namespace FlowCare.Application.DTOs.Requests;
public sealed record BookAppointmentRequest(
    int BranchId,
    int ServiceTypeId,
    int SlotId,
    int CustomerId,
    int StaffId
    )
{
    [FileTypeValidator(new[] { ".png", ".jpg", ".jpeg",".pdf" })]
    [FileSizeValidator(1,10)]
    public IFormFile? formFile { get; init; }
}

public sealed record RescheduleAppointmentRequest(int NewSlotId);

public sealed record UpdateAppointmentStatusRequest(AppointmentStatus Status);