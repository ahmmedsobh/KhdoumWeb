using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Models
{
    public class Member
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل الاسم")]
        [DisplayName("الاسم")]
        public string FullName { get; set; }

        public string ShortLink { get; set; }
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
        public string ImgUrl { get; set; }
        [DisplayName("الحالة")]
        public bool State { get; set; }
        [DisplayName("كود التفعيل")]
        public string ActivationCode { get; set; }
        [DisplayName("عدد العناصر")]
        public int ItemsCount { get; set; }
        public List<Item> Items { get; set; }
        public List<MemberItem> MembersItems { get; set; }
        public List<MemberRole> MembersRoles { get; set; }
        public List<Raiting> Raitings { get; set; }
        public List<Comment> Comments { get; set; }
        public int CityId { get; set; }
        [DisplayName("المدينة")]
        public City City { get; set; }
    }
}
