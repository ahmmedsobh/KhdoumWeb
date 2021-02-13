using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string DeliveryDate { get; set; }
        public string Address { get; set; }
        public decimal Total { get; set; }
        public string Notes { get; set; }
        public string Phone { get; set; }
        public string State { get; set; }
        public string LoginedPhone { get; set; }
        public decimal DeliveryService { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
    }
}
