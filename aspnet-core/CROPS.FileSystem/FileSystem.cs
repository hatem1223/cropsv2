using Abp.Dependency;
using CROPS.Configuration;
using CROPS.Storage;
using CROPS.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CROPS.FileSystem
{
    public class FileSystem : IStorageProvider, ITransientDependency
    {
        private readonly string physicalPath;
        private readonly string webPath;

        public FileSystem(IConfiguration configuration)
        {
            physicalPath = configuration.GetSection("StorageSettings:PhysicalPath").Value;
            webPath = configuration.GetSection("StorageSettings:WebURL").Value;
        }
        public async Task<string> Save(Stream stream, string fileName)
        {
            var fullPath = Path.Combine(physicalPath, fileName);

            using (var fileStream = File.Create(fullPath))
            {
                stream.Seek(0, SeekOrigin.Begin);
                await stream.CopyToAsync(fileStream);
                return Path.Combine(webPath, fileName); ;
            }
        }
    }
}
