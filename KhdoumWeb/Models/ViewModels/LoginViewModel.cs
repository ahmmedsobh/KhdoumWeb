using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Models.ViewModels
{
    public class LoginViewModel
    {
        [DisplayName("البريد الالكترونى أو رقم الهاتف")]
        [Required(ErrorMessage = "من فضلك ادخل رقم الموبيل")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل كلمة المرور")]
        [DisplayName("كلمة المرور")]
        public string Password { get; set; }

        public string Url { get; set; }
    }
}
