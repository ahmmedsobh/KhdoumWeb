using AutoMapper;
using KhdoumWeb.Models;
using KhdoumWeb.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Profiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Item, ItemViewModel>();
            CreateMap<ItemViewModel, Item>();
        }


    }
}
