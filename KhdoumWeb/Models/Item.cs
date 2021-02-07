using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل العنوان")]
        [MinLength(30)]
        [DisplayName("العنوان")]
        public string Title { get; set; }
        [DisplayName("التاريخ")]
        public string Date { get; set; }
        [Required]
        [DisplayName("رابط الصورة")]
        public string ImgUrl { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل الوصف")]
        [MinLength(100)]
        [DisplayName("الوصف")]
        public string Description { get; set; }

        [Required]
        [DisplayName("الموقع")]
        public string Address { get; set; }

        [DisplayName("الحالة")]
        public bool State { get; set; }

        //[Required]
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
        [Phone]
        public string Phone { get; set; }


        [DisplayName("الخريطة")]
        public string LocationMap { get; set; }



        [Required(ErrorMessage = "معدل زيادة الكمية مطلوب")]
        [DisplayName("معدل زيادة الكمية")]
        public decimal QuantityDuration { get; set; }


        public int MemberId { get; set; }
        [DisplayName("المورد")]
        public Member Member { get; set; }



        public List<ItemCategory> ItemsCategories { get; set; }
        public List<MemberItem> MembersItems { get; set; }
        public List<ItemSupCategory> ItemsSupCategories { get; set; }
        public List<Raiting> Raitings { get; set; }
        public List<Comment> Comments { get; set; }

        public int CityId { get; set; }
        [DisplayName("المدينة")]
        public City City { get; set; }

        [DisplayName("الوحده")]
        public int? UnitId { get; set; }
        [DisplayName("الوحدة")]
        public Unit Unit { get; set; }
       
    }
}
