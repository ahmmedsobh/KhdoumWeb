using AutoMapper;
using KhdoumWeb.Models;
using KhdoumWeb.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Profiles
{
    public class MemberProfile : Profile
    {
        public MemberProfile()
        {
            CreateMap<Member, MemberViewModel>();
            CreateMap<MemberViewModel, Member>();
        }
    }
}
