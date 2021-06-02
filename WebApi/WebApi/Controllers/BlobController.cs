using Azure.Core;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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
    [Route("api/blobs")]
    [ApiController]
    public class BlobController : ControllerBase
    {
        private BlobStorageSettings _blobStorageSettings;

        public BlobController(BlobStorageSettings blobStorageSettings)
        {
            this._blobStorageSettings = blobStorageSettings;
        }

        [HttpGet(Name ="GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var files = await Files();
                return Ok(files);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpPost(Name = "UploadBlob")]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var files = Request.Form.Files;

                //support uploading of multiple files
                foreach (var file in files)
                {
                   await UploadFile(file);

                    //await UploadFileUserAssigned(file);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{resourceId}", Name = "DeleteBlob")]
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

        [HttpGet("{resourceId}",Name = "DownloadBlob")]
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
            await containerClient.UploadBlobAsync($"{_blobStorageSettings.SubFolderPath}/{resourceId}", file.OpenReadStream());
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

        private async Task DeleteFile( string resourceId)
        {

            var client = new DefaultAzureCredential();

            BlobContainerClient containerClient =
                                           new BlobContainerClient(new Uri(_blobStorageSettings.BlobContainerEndPoint),
                                           client);

            var blobClient =  containerClient.GetBlobClient(resourceId);

            await blobClient.DeleteIfExistsAsync();
        }


        private async Task<List<string>> Files(int? pageSize=5)
        {
            var files = new List<string>();

            var client = new DefaultAzureCredential();

            BlobContainerClient containerClient =
                                 new BlobContainerClient(new Uri(_blobStorageSettings.BlobContainerEndPoint),
                                 client);


            await foreach (BlobItem blob in containerClient.GetBlobsAsync())
            {
                files.Add(blob.Name);

                if (files.Count > pageSize)
                {
                    break;
                }
            }

            return files;
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
