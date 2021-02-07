using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Models
{
    public class Unit
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "الاسم مطلوب")]
        [DisplayName("اسم الوحدة")]
        public string Name { get; set; }
        [DisplayName("الوصف")]
        public string Note { get; set; }

        public List<Item> Items { get; set; }
    }
}
