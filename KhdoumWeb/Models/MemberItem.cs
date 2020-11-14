using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Models
{
    public class MemberItem
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
