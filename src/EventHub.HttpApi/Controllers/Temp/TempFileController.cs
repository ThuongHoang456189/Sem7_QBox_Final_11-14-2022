using EventHub.BlobContainer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace EventHub.Controllers.Temp
{
    public class TempFileController : AbpController
    {
        private readonly IQBoxFileAppService _fileAppService;

        public TempFileController(IQBoxFileAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFiles([FromForm] List<IFormFile> files)
        {
            foreach (var f in files)
            {
                string name = f.FileName.Replace(@"\\\\", @"\\");

                if (f.Length > 0)
                {
                    var memoryStream = new MemoryStream();

                    await f.CopyToAsync(memoryStream);

                    await _fileAppService.SaveBlobAsync(new SaveBlobInputDto
                    {
                        Name = "LanVaDiep",
                        File = memoryStream.ToArray()
                    });
                }
            }
            return Ok("xong roi");
        }
    }
}
