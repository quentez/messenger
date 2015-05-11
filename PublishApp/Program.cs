using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace PublishApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() => MainAsync()).Wait();
        }

        private static async Task MainAsync()
        {
            // Create the storage account we'll be using.
            var connectionString = ConfigurationManager.AppSettings["StorageAccountConnectionString"];
            var storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the blobs client and make sure the container exists.
            var blobsClient = storageAccount.CreateCloudBlobClient();
            var appContainer = blobsClient.GetContainerReference("app");

            await appContainer.CreateIfNotExistsAsync();

            // Set public access on container.
            await appContainer.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            // Upload the files.
            var basePath = ConfigurationManager.AppSettings["PublishPath"];
            var basePathUri = new Uri(basePath);
            var files = Directory.EnumerateFiles(basePath, "*.*", SearchOption.AllDirectories);

            var uploadTasks = files.Select(async file =>
            {
                var fileUri = new Uri(file);
                var fileRelativePath = basePathUri.MakeRelativeUri(fileUri)
                    .OriginalString
                    .Replace("%20", " ");

                // Open the file.
                using (var fileStream = File.OpenRead(file))
                {
                    // If this is the setup file, rename it.
                    if (fileRelativePath == "setup.exe")
                        fileRelativePath = "messenger-setup.exe";

                    // Get a reference to our blob.
                    var blob = appContainer.GetBlockBlobReference(fileRelativePath);

                    // If the blob already exists, we may be able to skip it.
                    var blobExists = await blob.ExistsAsync();
                    if (blobExists && fileRelativePath.StartsWith("Application"))
                        return;

                    // Upload it.
                    await blob.UploadFromStreamAsync(fileStream);

                    Console.WriteLine("Done uploading file {0}.", fileRelativePath);

                    // If needed, set its Cache-Control.
                    if (!fileRelativePath.StartsWith("Application"))
                    {
                        blob.Properties.CacheControl = "no-cache";
                        await blob.SetPropertiesAsync();
                    }
                }
            });

            // Wait on all those tasks to complete.
            await Task.WhenAll(uploadTasks);

            Console.WriteLine("Done uploading.");
            Console.ReadKey();
        }
    }
}
