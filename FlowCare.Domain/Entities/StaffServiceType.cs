namespace FlowCare.Domain.Entities;

public sealed class StaffServiceType
{
    public int Id { get; init; }
    public int StaffId { get; init; }
    public int ServiceTypeId { get; init; }

    public Staff? Staff { get; init; }
    public ServiceType? ServiceType { get; init; }
}
