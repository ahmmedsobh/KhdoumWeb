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
    public class MemberViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل الاسم")]
        [DisplayName("الاسم")]
        public string FullName { get; set; }
        [EmailAddress]
        [DisplayName("البريد الالكترونى")]
        public string Email { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل رقم الهاتف")]
        [DisplayName("رقم الهاتف")]
        [Phone]
        public string Phone { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل كلمة المرور")]
        [DisplayName("كلمة المرور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "من فضلك قم بتأكيد كلمة المرور")]
        [DisplayName("كلمة المرور")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمة ليست متطابقة")]
        public string ConfirmPassword { get; set; }
        [DisplayName("الصورة")]
        [MaxFileSize(2 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile File { get; set; }
        [DisplayName("الحالة")]
        public bool State { get; set; }
        [DisplayName("كود التفعيل")]
        public string ActivationCode { get; set; }
        [DisplayName("عدد العناصر")]
        [Required(ErrorMessage = "ادخل عدد العناصر")]

        public int ItemsCount { get; set; }

        [Required(ErrorMessage = "من فضلك اختر المدينة")]
        [DisplayName("المدينة")]
        public int CityId { get; set; }
    }
}
