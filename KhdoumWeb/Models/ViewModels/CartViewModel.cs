using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Models.ViewModels
{
    public class CartViewModel
    {
        public string UserPhone { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public decimal Quantity { get; set; }
        public decimal Value { get; set; }
        public int ItemId { get; set; }
        public int UnitId { get; set; }
    }
}
