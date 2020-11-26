using KhdoumWeb.CustomValidationAttribute;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Models.ViewModels
{
    public class SubCategoryViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل الاسم")]
        [DisplayName("الاسم")]
        public string Name { get; set; }
       // [Required(ErrorMessage = "من فضلك ادخل الوصف")]
        [DisplayName("الوصف")]
        public string Description { get; set; }

        [DisplayName("الصورة")]
        [MaxFileSize(2 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile File { get; set; }

        [DisplayName("الحالة")]
        public bool State { get; set; }
    }
}
