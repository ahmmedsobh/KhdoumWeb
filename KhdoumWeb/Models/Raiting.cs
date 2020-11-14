using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Models
{
    public class Raiting
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل رسالتك")]
        [DisplayName("رسالتك")]
        public string Content { get; set; }

        [Required(ErrorMessage = "اختر قيمة التقييم")]
        [DisplayName("قيمة التقييم")]
        public byte Value { get; set; }
        public bool State { get; set; }


        public int MemberId { get; set; }
        public Member Member { get; set; }
        public int ItemId { get; set; }

        public Item Item { get; set; }
    }
}
