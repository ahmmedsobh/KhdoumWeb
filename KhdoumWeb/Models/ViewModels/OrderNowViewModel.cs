using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Models.ViewModels
{
    public class OrderNowViewModel
    {
        [Required(ErrorMessage = "من فضلك قم بادخال الاسم")]
        public string UserName { get; set; }
        public string LoginedUserPhone { get; set; }
        [Required(ErrorMessage = "من فضلك قم بادخال رقم الهاتف")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "رقم هاتف غير صالح")]
        [RegularExpression(@"^([0-9]{11})$", ErrorMessage = "رقم هاتف غير صالح")]
        public string UserPhone { get; set; }
        public string Password { get; set; }
        [Required(ErrorMessage = "من فضلك قم باختيار مدينة")]
        public int CityId { get; set; }
        [Required(ErrorMessage = "من فضلك قم بادخال  عنوانك بالتفصيل")]
        public string Address { get; set; }
        public string Notes { get; set; }
        public decimal Total { get; set; }
        public List<City> Cities { get; set; }
    }
}
