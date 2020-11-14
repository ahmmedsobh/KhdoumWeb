using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Models.ViewModels
{
    public class ItemDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string ImgUrl { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string ShortLink { get; set; }
        public int? ViewsCount { get; set; }
        public int? RatingValues { get; set; }
        public int? RatingCount { get; set; }
        public float? Price { get; set; }
        public bool IsPaid { get; set; }
        public string Phone { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }


        public Member Member { get; set; }
    }
}
