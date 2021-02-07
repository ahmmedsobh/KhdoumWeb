using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public int UnitId { get; set; }
        public Unit Unit { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; }

        public int CartId { get; set; }
        public CartHeader CartHeader { get; set; }

    }
}
