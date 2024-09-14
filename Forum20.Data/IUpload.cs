using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forum20.Data
{
   public interface IUpload
    {
        CloudBlobContainer GetBlobContainer(string connectionString,string containerName);

    }
}
