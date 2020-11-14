using AutoMapper;
using KhdoumWeb.Models;
using KhdoumWeb.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryViewModel, Category>();

            //CreateMap<Category,CategoryViewModel>().ForMember(x => x.Id, opt => opt.Ignore()); 
            //CreateMap<CategoryViewModel,Category>().ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
