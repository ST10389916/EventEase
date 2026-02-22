using Azure.Storage.Blobs;


namespace EventEaseWebApp.Data
{
    public class BlobService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobService(IConfiguration config)
        {
            var connection = config["AzureBlobStorage:ConnectionString"];
            var container = config["AzureBlobStorage:ContainerName"];
            _containerClient = new BlobContainerClient(connection, container);
            _containerClient.CreateIfNotExists();
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            var blob = _containerClient.GetBlobClient(Guid.NewGuid() + Path.GetExtension(file.FileName));
            await using var stream = file.OpenReadStream();
            await blob.UploadAsync(stream);
            return blob.Uri.ToString();
        }
    }


}
