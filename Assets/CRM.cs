using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace WorkAppReactAPI.Assets
{
    public static class CRM
    {
        public static string UploadImage(this IWebHostEnvironment _hostingEnvironment, IFormFile file, string path)
        {
            string ext = Path.GetExtension(file.FileName).ToLower();
            if (ext.Contains(".php") || ext.Contains(".asp") || ext.Contains(".aspx") || ext.Contains(".ps1"))
            {
                return null;
            }
            var fileName = Guid.NewGuid() + ext; 
            var filepath = _hostingEnvironment.WebRootPath + path + fileName;
            if (!Directory.Exists(_hostingEnvironment.WebRootPath + filepath))
            {
                Directory.CreateDirectory(_hostingEnvironment.WebRootPath + filepath);
            }
            using (FileStream filetream = System.IO.File.Create(filepath))
            {
                file.CopyTo(filetream);
                filetream.Flush();
                var result= path + fileName;
                return result.Replace(@"\", "/"); ;
            }
        }
    }
}