using Microsoft.AspNetCore.Http;

namespace FlowCare.Application.Interfaces.Services_Interfaces;

public interface IFileStorageService
{
    Task<string> StoreCustomerIdImageAsync(IFormFile file, CancellationToken ct = default);
    (Stream FileStream, string ContentType, string FileName) RetrieveCustomerIdImageAsync(string fileReference);
    
    Task<string> StoreAppointmentAttachmentAsync(IFormFile file, CancellationToken ct = default);
    (Stream FileStream, string ContentType, string FileName) RetrieveAppointmentAttachmentAsync(string fileReference);
}