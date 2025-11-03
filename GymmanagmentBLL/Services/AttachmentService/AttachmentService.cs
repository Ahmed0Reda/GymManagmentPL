using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymmanagmentBLL.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {

        private readonly string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly long maxFileSize = 5 * 1024 * 1024; // 5 MB
        private readonly IWebHostEnvironment _webHost;
        public AttachmentService(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }

        public string? Upload(string folderName, IFormFile file)
        {
            try
            {
                if (folderName is null || file is null || file.Length == 0) return null;
                if (file.Length > maxFileSize) return null;
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension)) return null;
                var folderPath = Path.Combine(_webHost.WebRootPath, "images", folderName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var fileName = Guid.NewGuid().ToString() + fileExtension;
                var filePath = Path.Combine(folderPath, fileName);
                using var filestream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(filestream);
                return fileName;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Faild To Upload File To Folder = {folderName} : {ex}");
                return null;
            }

        }
        public bool Delete(string fileName, string folderName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName))
                    return false;
                var fullPath = Path.Combine(_webHost.WebRootPath, "images", folderName, fileName);
                if(File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Faild To Delete File with Name = {fileName} : {ex}");
                return false;
            }
        }

    }
}
