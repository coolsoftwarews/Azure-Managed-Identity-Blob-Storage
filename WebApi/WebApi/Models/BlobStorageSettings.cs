using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
      public class BlobStorageSettings
    {
        /// <summary>
        /// only needed when using a User Assigned Identity
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Name of Storage Account
        /// </summary>
        public string StorageAccount { get; set; }

        /// <summary>
        /// Name of Container or path to container subfolders
        /// </summary>
        public string BlobContainer { get; set; }

        /// <summary>
        /// generated storage account end point
        /// </summary>
        public string ContainerEndPoint
        {
            get
            {
                return $"https://{StorageAccount}.blob.core.windows.net/{BlobContainer}";
            }
        }
    }
}
