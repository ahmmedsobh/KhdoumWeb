using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Helpers
{
    public class UploadImages
    {
        private readonly IWebHostEnvironment hosting;


        public string Folder { get; set; }
        public UploadImages(IWebHostEnvironment hosting)
        {
            this.hosting = hosting;


        }

        public string AddImage(IFormFile file)
        {
            if (file != null)
            {

                string FileName = FullFileName(file.FileName);
                string FullPath = NewPath(FileName);
                SaveImage(file, FullPath);
                return FileName;
            }
            else
            {
                return "false";
            }

        }
        void SaveImage(IFormFile file, string FullPath)
        {

            file.CopyTo(new FileStream(FullPath, FileMode.Create));
        }
        public string UpdateImage(string ImgUrl, IFormFile file)
        {
            if (file != null)
            {
                string FileName = FullFileName(file.FileName);
                string oldPath = OldPath(ImgUrl);
                string newPath = NewPath(FileName);
                if (newPath != oldPath)
                {
                    DeleteImage(ImgUrl);
                    SaveImage(file, newPath);
                }
                return FileName;
            }
            return ImgUrl;
        }
        public void DeleteImage(string ImgUrl)
        {
            if (ImgUrl != null && ImgUrl != "false")
            {
                string FullPath = OldPath(ImgUrl);
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                System.IO.File.Delete(FullPath);
            }

        }
        string OldPath(string ImgUrl)
        {
            string oldFullPath = Path.Combine(Uploads(), ImgUrl);
            return oldFullPath;
        }
        string Uploads()
        {
            string uplaods = Path.Combine(hosting.WebRootPath, $"uploads/{Folder}");
            return uplaods;
        }
        string NewPath(string FileName)
        {
            string NewPath = Path.Combine(Uploads(), FileName);
            return NewPath;
        }

        string FullFileName(string FileName)
        {
            string FullFileName = Guid.NewGuid().ToString() + Path.GetExtension(FileName);
            return FullFileName;
        }
    }
}
