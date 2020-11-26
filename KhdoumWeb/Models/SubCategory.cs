using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Models
{
    public class SubCategory
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل الاسم")]
        [DisplayName("الاسم")]
        public string Name { get; set; }
        //[Required(ErrorMessage = "من فضلك ادخل الوصف")]
        [DisplayName("الوصف")]
        public string Description { get; set; }
        [Required]
        [DisplayName("الصورة")]

        public string ImgUrl { get; set; }
        [Required]
        [DisplayName("الحالة")]

        public bool State { get; set; }

        public List<ItemSupCategory> ItemsSupCategories { get; set; }
    }
}
