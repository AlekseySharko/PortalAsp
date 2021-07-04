using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using PortalAsp.Validators.Catalog;

namespace PortalAsp.Controllers.Files
{
    [Route("api/files")]
    public class FileController : Controller
    {
        private readonly IHostEnvironment _hostEnvironment;

        public FileController(IHostEnvironment environment)
        {
            _hostEnvironment = environment;
        }

        [HttpPost]
        [Route("upload-image")]
        public async Task<IActionResult> SaveImage(IFormFileCollection files)
        {
            if (files is null || files.Count < 1)
            {
                return BadRequest("No files to save");
            }

            if (files.Count > 10)
            {
                return BadRequest("Too many files");
            }

            var uploads = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot/uploads");
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            List<string> resultUrls = new List<string>();
            foreach (IFormFile file in files)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                if(ImageAddressValidator.CheckExtension(fileExtension) == false) 
                {
                    return BadRequest($"{file.FileName} is not an image");
                }

                if (file.Length > 0)
                {
                    string fileName = RandomString(10);
                    string fullName = fileName + fileExtension;
                    var filePath = Path.Combine(uploads, fullName);
                    await using var fileStream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(fileStream);
                    resultUrls.Add($"{Request.Host}/uploads/{fullName}");
                }
            }
            return Ok(resultUrls);
        }
        public string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
