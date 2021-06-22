using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
      public class DataLakeAccountSettings
    {
        /// <summary>
        /// only needed when using a User Assigned Identity
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Name of Storage Account
        /// </summary>
        public string StorageAccountName { get; set; }

        /// <summary>
        /// Name of Container
        /// </summary>
        public string ContainerName { get; set; }

        /// <summary>
        /// Name and folder path
        /// </summary>
        public string SubFolderPath { get; set; }

        /// <summary>
        /// generated storage account end point
        /// </summary>

        public string DataLakesEndPoint
        {
            get
            {
                return $"https://{StorageAccountName}.dfs.core.windows.net";
            }
        }
    }
}
