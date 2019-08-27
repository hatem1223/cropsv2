using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CROPS.Configuration;
using CROPS.Controllers;
using CROPS.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CROPS.Web.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : CROPSControllerBase
    {
        private readonly IStorageProvider storageProvider;
        private readonly string imageContentType;
        private readonly string imageSizeLimitinMB;
        private int imageSizeLimitinByte;

        public FilesController(IConfiguration configuration, IStorageProvider storageProvider)
        {
            this.storageProvider = storageProvider;
            imageContentType = configuration.GetSection("StorageSettings:imageContentType").Value;
            imageSizeLimitinMB = configuration.GetSection("StorageSettings:imageSizeLimitinMB").Value;
            imageSizeLimitinByte = Convert.ToInt32(imageSizeLimitinMB) * 1024;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            var file = Request.Form.Files[0];
            if (file.Length > 0 && (file.Length / 1024) < imageSizeLimitinByte && file.ContentType.StartsWith(imageContentType))
            {
                var path = await storageProvider.Save(file.OpenReadStream(), file.FileName);
                return Ok(new { path });
            }
            else
            {
                return BadRequest();
            }
        }
    }
}