using EventHub.BlobContainer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpGet]
        [Route("download/{fileName}")]
        public async Task<IActionResult> DownloadAsync(string fileName)
        {
            fileName = fileName.Replace(@"%2F", @"/");

            var fileDto = await _fileAppService.GetBlobAsync(new GetBlobRequestDto { Name = fileName });

            return File(fileDto.File, "application/octet-stream", fileDto.Name);
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
                        Name = name,
                        File = memoryStream.ToArray()
                    });
                }
            }
            return Ok("xong roi");
        }

        private byte[] TransferIntArrayToByteArray(int[] array)
        {
            byte[] result = new byte[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = byte.Parse(array[i] + "");
            }
            return result;
        }

        [HttpGet]
        [Route("testbyte")]
        public async Task<IActionResult> Post(int[] bytes)
        {
            byte t;
            if (bytes != null)
            {
                return Ok(TransferIntArrayToByteArray(bytes)[0]);
            }
            return Ok(bytes is null ? "Null ba" : "Tui ko bit");
        }
    }
}
