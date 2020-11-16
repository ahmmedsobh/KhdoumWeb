using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Models.ViewModels
{
    public class ItemViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل العنوان")]
        [MinLength(30)]
        [DisplayName("العنوان")]
        public string Title { get; set; }
        [DisplayName("التاريخ")]
        public string Date { get; set; }
        [DisplayName("الصورة")]
        public IFormFile File { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل الوصف")]
        [MinLength(100)]
        [DisplayName("الوصف")]
        public string Description { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل الموقع")]
        [DisplayName("الموقع")]
        public string Address { get; set; }

        [DisplayName("الحالة")]
        public bool State { get; set; }

        [Required]
        [DisplayName("الخصوصية")]
        public string Privacy { get; set; }

        [DisplayName("الرابط المختصر")]
        public string ShortLink { get; set; }

        [DisplayName("عدد النقرات")]
        public int? ClicksCount { get; set; }

        [DisplayName("عدد المشاهدات")]
        public int? ViewsCount { get; set; }
        [DisplayName("قيمة التقييمات")]

        public int? RatingValues { get; set; }
        [DisplayName("عدد التقييمات")]

        public int? RatingCount { get; set; }
        [DisplayName("عدد الاعضاء")]

        public int? MembersCount { get; set; }
        [DisplayName("عدد من وضعوك فى المفضلة")]

        public int? FavoriteCount { get; set; }

        [DisplayName("الترتيب")]
        public int? Sorting { get; set; }

        [DisplayName("السعر")]
        public float? Price { get; set; }

        [DisplayName("حالة الخدمة")]
        public bool IsPaid { get; set; }

        [Required(ErrorMessage = "ادخل رقم الهاتف")]
        [DisplayName("رقم الهاتف")]
        [Phone(ErrorMessage = "ادخل رقم هاتف صحيح")]
        public string Phone { get; set; }

        [DisplayName("صاحب الخدمة")]
        [Required(ErrorMessage = "اختر صاحب الخدمة")]

        public int MemberId { get; set; }
        [DisplayName("المدينة")]
        [Required(ErrorMessage = "اختر المدينة")]
        public int CityId { get; set; }

        [DisplayName("التقييم")]
        [Required(ErrorMessage = "اختر تقييم")]
        public int RaitingValue { get; set; }

    }
}
