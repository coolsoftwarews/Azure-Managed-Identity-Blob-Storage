using Azure.Core;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Files.DataLake;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/documents")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private BlobStorageSettings _blobStorageSettings;

        public DocumentsController(BlobStorageSettings blobStorageSettings)
        {
            this._blobStorageSettings = blobStorageSettings;
        }


        [HttpPost(Name = "Upload")]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var files = Request.Form.Files;

                //support uploading of multiple files
                foreach (var file in files)
                {
                    await TaskUploadFileDataLakes(file);

                    // await UploadFile(file);

                    //await UploadFileUserAssigned(file);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{resourceId}", Name = "Delete")]
        public async Task<IActionResult> Delete(string resourceId)
        {
            try
            {
                await DeleteFile(resourceId);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "Download")]
        public async Task<IActionResult> Download(string resourceId)
        {
            try
            {
                return await DownloadFile(resourceId);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        private async Task TaskUploadFileDataLakes(IFormFile file)
        {
            string[] fileParts = file.FileName.Split('.');
            string resourceId = $"D-{Guid.NewGuid()}.{fileParts[fileParts.Length - 1]}";

            var client = new DefaultAzureCredential();

            var dataLakeServiceClient =
                                        new DataLakeServiceClient
                                        (new Uri(_blobStorageSettings.DataLakesEndPoint), client);


            var fileSystemClient = dataLakeServiceClient.GetFileSystemClient(_blobStorageSettings.BlobContainer);
            var directoryClient = fileSystemClient.GetDirectoryClient("SubFolder");
            var fileClient = directoryClient.GetFileClient(resourceId);
            
            await fileClient.UploadAsync(file.OpenReadStream());

        }

        /// <summary>
        /// Upload file using System Assigned Managed Identity e.g. App Service
        /// </summary>
        /// <param name="fileBinary"></param>
        /// <returns></returns>
        private async Task UploadFile(IFormFile file)
        {
            string[] fileParts = file.FileName.Split('.');
            string resourceId = $"D-{Guid.NewGuid()}.{fileParts[fileParts.Length - 1]}";

            var client = new DefaultAzureCredential();

            BlobContainerClient containerClient =
                                           new BlobContainerClient(new Uri(_blobStorageSettings.BlobContainerEndPoint),
                                           client);

            await containerClient.CreateIfNotExistsAsync();
            await containerClient.UploadBlobAsync($"SubFolder/{resourceId}", file.OpenReadStream());
        }

        /// <summary>
        /// Upload files Using User Assgined Managed Identity
        /// </summary>
        /// <param name="fileBinary"></param>
        /// <returns></returns>
        private async Task UploadFileUserAssigned(IFormFile file)
        {
            string[] fileParts = file.FileName.Split('.');
            string resourceId = $"D-{Guid.NewGuid()}.{fileParts[fileParts.Length - 1]}";

            var client = new DefaultAzureCredential(
                            new DefaultAzureCredentialOptions { ManagedIdentityClientId = _blobStorageSettings.ClientId });

            BlobContainerClient containerClient =
                                    new BlobContainerClient(new Uri(_blobStorageSettings.BlobContainerEndPoint),
                                    client
                                    );

            await containerClient.CreateIfNotExistsAsync();
            await containerClient.UploadBlobAsync(resourceId, file.OpenReadStream());
        }

        private async Task DeleteFile(string resourceId)
        {
            var client = new DefaultAzureCredential();

            BlobContainerClient containerClient =
                                           new BlobContainerClient(new Uri(_blobStorageSettings.BlobContainerEndPoint),
                                           client);

            await containerClient.DeleteAsync();
        }

        private async Task<FileStreamResult> DownloadFile(string resourceId)
        {
            var client = new DefaultAzureCredential();

            BlobContainerClient containerClient =
                                           new BlobContainerClient(new Uri(_blobStorageSettings.BlobContainerEndPoint),
                                           client);

            var blobClient = containerClient.GetBlobClient(resourceId);

            var downloadInfo = await blobClient.DownloadAsync();

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(resourceId, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            FileStreamResult fileResult = new FileStreamResult(downloadInfo.Value.Content, contentType);
            fileResult.FileDownloadName = resourceId;

            return fileResult;
        }
    }
}
