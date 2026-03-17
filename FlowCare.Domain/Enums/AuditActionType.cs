namespace FlowCare.Domain.Enums;

public enum AuditActionType
{
    BookAppointment = 1,
    CancelAppointment = 2,
    RescheduleAppointment = 3,
    UpdateAppointmentStatus = 4,
    CreateSlot = 5,
    UpdateSlot = 6,
    SoftDeleteSlot = 7,
    AssignStaffService = 8,
    AssignStaffBranch = 9,
    HardDeleteExpiredSlots = 10,
    CreateBranch = 11,
    UpdateBranch = 12
}
