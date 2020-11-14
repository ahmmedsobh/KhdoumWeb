using AutoMapper;
using KhdoumWeb.Models;
using KhdoumWeb.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Profiles
{
    public class SubCategoryProfile : Profile
    {
        public SubCategoryProfile()
        {
            CreateMap<SubCategory, SubCategoryViewModel>();
            CreateMap<SubCategoryViewModel, SubCategory>();

            //CreateMap<SubCategory, SubCategoryViewModel>().ForMember(x => x.Id, opt => opt.Ignore());
            //CreateMap<SubCategoryViewModel, SubCategory>().ForMember(x => x.Id, opt => opt.Ignore());
        }

    }
}
