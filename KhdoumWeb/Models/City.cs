using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Models
{
    public class City
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "من فضلك اسم الاسم")]
        [DisplayName("الاسم")]
        public string Name { get; set; }
        public int GovernorateId { get; set; }
        public Governorate Governorate { get; set; }
        public List<Item> Items { get; set; }
        public List<Member> Members { get; set; }
    }
}
