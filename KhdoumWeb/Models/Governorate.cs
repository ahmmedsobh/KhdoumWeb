using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Models
{
    public class Governorate
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "من فضلك اسم الاسم")]
        [DisplayName("الاسم")]
        public string Name { get; set; }

        public List<City> Cities { get; set; }
    }
}
