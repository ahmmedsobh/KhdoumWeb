using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Models
{
    public class Role
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="هذا الحقل مطلوب")]
        [DisplayName("اسم الصلاحية")]
        public string Name { get; set; }

        public List<MemberRole> MembersRoles { get; set; }
    }
}
