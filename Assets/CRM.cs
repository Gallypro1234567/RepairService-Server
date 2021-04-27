using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace WorkAppReactAPI.Assets
{
    public static class CRM
    {
        public static string UploadImage(this IWebHostEnvironment _hostingEnvironment, IFormFile file, string folderpath)
        {
            try
            {
                string ext = Path.GetExtension(file.FileName).ToLower();
                if (ext.Contains(".php") || ext.Contains(".asp") || ext.Contains(".aspx") || ext.Contains(".ps1"))
                {
                    return null;
                }
                var fileName = Guid.NewGuid() + ext;
              
                if (!Directory.Exists(_hostingEnvironment.WebRootPath + folderpath))
                {
                    Directory.CreateDirectory(_hostingEnvironment.WebRootPath + folderpath);
                }
                using (FileStream filetream = System.IO.File.Create(_hostingEnvironment.WebRootPath + folderpath + fileName))
                {
                    file.CopyTo(filetream);
                    filetream.Flush();
                    var link =folderpath + fileName;
                    return link.Replace(@"\", "/"); 
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public static bool DeleteImage(this IWebHostEnvironment _hostingEnvironment,  string link)
        {
            try
            {   

                string path  = link.Replace(@"\", "/");
                 System.IO.File.Delete(_hostingEnvironment.WebRootPath + path);
                 return true; 
            }
            catch 
            {
                return false;
            }

        }
    }
}