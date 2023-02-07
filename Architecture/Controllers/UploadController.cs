using Architecture.Core.Services.Files;
using Architecture.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Architecture.Controllers
{
    [Route("api/upload")]
    [ApiController]
    public class UploadController : Controller
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IFileService _fileService;

        public UploadController(
            IWebHostEnvironment appEnvironment,
            IFileService fileService
        )
        {
            _appEnvironment = appEnvironment;
            _fileService = fileService;
        }

        /// <summary>
        /// Upload file
        /// </summary>
        /// <returns></returns>
        [HttpPost("file")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                var userId = 1.ToString();
                var folderName = Path.Combine("wwwroot", "files", userId);
                
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    var relativePath = Path.Combine("files", userId, fileName);

                    // create directory if not exist
                    Directory.CreateDirectory(pathToSave);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    await _fileService.Create(new Domain.Entities.File
                    {
                        FileName = fileName,
                        AbsolutePath = fullPath,
                        Path = relativePath,
                    });

                    return Ok( new
                    {
                        Path = HttpContext.Request.Host.Value + "/files/" + userId + "/" + fileName,
                        Path2 = $"{HttpContext.Request.Host.Value}/files/{userId}/{fileName}",
                    });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Test save form with file and other params
        /// </summary>
        /// <returns></returns>
        [HttpPost("upload-form")]
        public async Task<IActionResult> Upload2(
            [FromForm] UploadForm uploadForm
        )
        {
            return Ok();
        }
    }

    public class UploadForm
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
        public IFormFileCollection Files { get; set; }
    }
}
