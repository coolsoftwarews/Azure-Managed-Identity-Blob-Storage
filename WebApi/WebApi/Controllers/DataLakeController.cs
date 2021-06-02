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
    [Route("api/datalakes")]
    [ApiController]
    public class DataLakeController : ControllerBase
    {
        private BlobStorageSettings _blobStorageSettings;

        public DataLakeController(BlobStorageSettings blobStorageSettings)
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
                    await UploadFile(file);
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

            var dataLakeServiceClient =
                                        new DataLakeServiceClient
                                        (new Uri(_blobStorageSettings.DataLakesEndPoint), client);


            var fileSystemClient = dataLakeServiceClient.GetFileSystemClient(_blobStorageSettings.BlobContainer);
            var directoryClient = fileSystemClient.GetDirectoryClient("SubFolder");
            var fileClient = directoryClient.GetFileClient(resourceId);
            
            await fileClient.UploadAsync(file.OpenReadStream());

        }

       
        private async Task DeleteFile(string resourceId)
        {

            var client = new DefaultAzureCredential();

            var dataLakeServiceClient =
                                        new DataLakeServiceClient
                                        (new Uri(_blobStorageSettings.DataLakesEndPoint), client);


            var fileSystemClient = dataLakeServiceClient.GetFileSystemClient(_blobStorageSettings.BlobContainer);
            var directoryClient = fileSystemClient.GetDirectoryClient("DataLake");
            var fileClient = directoryClient.GetFileClient(resourceId);

            await fileClient.DeleteIfExistsAsync();
        }

        private async Task<FileStreamResult> DownloadFile(string resourceId)
        {
            var client = new DefaultAzureCredential();

            var dataLakeServiceClient =
                                        new DataLakeServiceClient
                                        (new Uri(_blobStorageSettings.DataLakesEndPoint), client);


            var fileSystemClient = dataLakeServiceClient.GetFileSystemClient(_blobStorageSettings.BlobContainer);
            var directoryClient = fileSystemClient.GetDirectoryClient("DataLake");
            var fileClient = directoryClient.GetFileClient(resourceId);
               
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(resourceId, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var stream = await fileClient.OpenReadAsync();
            
            FileStreamResult fileResult = new FileStreamResult(stream, contentType);
            fileResult.FileDownloadName = resourceId;

            return fileResult;
        }
    }
}
