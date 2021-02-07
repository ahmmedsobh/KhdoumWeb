using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Models
{
    public class CartHeader
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Address { get; set; }
        public decimal Total { get; set; }
        public string Notes { get; set; }
        public string Phone { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public List<Cart> CartDetails { get; set; }
    }
}
