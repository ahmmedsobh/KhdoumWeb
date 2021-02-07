using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Models.ViewModels
{
    public class ClientRegisterViewModel
    {
        [Required(ErrorMessage ="من فضلك قم بادخال الاسم")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "من فضلك قم بادخال رقم الهاتف")]
        [DataType(DataType.PhoneNumber,ErrorMessage = "رقم هاتف غير صالح")]
        [RegularExpression(@"^([0-9]{11})$", ErrorMessage = "رقم هاتف غير صالح")]
        public string UserPhone { get; set; }
        public int  CityId { get; set; }
    }
}
