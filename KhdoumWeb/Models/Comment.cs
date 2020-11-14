using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "من فضلك أكتب تعليقك")]
        [DisplayName("التعليق")]
        public string Content { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public bool State { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; }
        public int ItemId { get; set; }

        public Item Item { get; set; }
    }
}
