using Azure.Storage.Blobs;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AppointmentService.Services;

public class BlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageService(IConfiguration configuration)
    {
        _blobServiceClient = new BlobServiceClient(configuration["Storage:ConnectionString"]);
    }

    public async Task UploadAsync(string containerName, string fileName, Stream content)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(content, overwrite: true);
    }
}