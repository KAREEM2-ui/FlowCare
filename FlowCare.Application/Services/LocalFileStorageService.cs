using FlowCare.Application.Interfaces.Services_Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace FlowCare.Infrastructure.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _baseStoragePath;
    private readonly FileExtensionContentTypeProvider _contentTypeProvider;

    public LocalFileStorageService()
    {
        _baseStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

        // mapper for file type and content-type
        _contentTypeProvider = new FileExtensionContentTypeProvider();

        // Ensure directories exist
        Directory.CreateDirectory(Path.Combine(_baseStoragePath, "CustomerIds"));
        Directory.CreateDirectory(Path.Combine(_baseStoragePath, "Appointments"));
    }

    public async Task<string> StoreCustomerIdImageAsync(IFormFile file, CancellationToken ct = default)
    {
        return await StoreFileAsync(file, "CustomerIds", ct);
    }

    public (Stream FileStream, string ContentType, string FileName) RetrieveCustomerIdImageAsync(string fileReference)
    {
        fileReference = Path.Combine("CustomerIds", fileReference);
        return GetFileDetails(fileReference);
    }

    public async Task<string> StoreAppointmentAttachmentAsync(IFormFile file, CancellationToken ct = default)
    {
        return await StoreFileAsync(file, "Appointments", ct);
    }

    public (Stream FileStream, string ContentType, string FileName) RetrieveAppointmentAttachmentAsync(string fileReference)
    {
        fileReference = Path.Combine("Appointments", fileReference);
        return GetFileDetails(fileReference);
    }

    private async Task<string> StoreFileAsync(IFormFile file, string folder, CancellationToken ct)
    {
        var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var relativePath = Path.Combine(folder, uniqueFileName);
        var fullPath = Path.Combine(_baseStoragePath, relativePath);

        using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream, ct);

        return relativePath.Replace('\\', '/');
    }

    private (Stream FileStream, string ContentType, string FileName) GetFileDetails(string fileReference)
    {
        var fullPath = Path.Combine(_baseStoragePath, fileReference.Replace('/', Path.DirectorySeparatorChar));

        if (!File.Exists(fullPath))
            throw new FileNotFoundException("The requested file was not found.");

        if (!_contentTypeProvider.TryGetContentType(fullPath, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        
        return (stream, contentType, Path.GetFileName(fullPath));
    }

    
} 