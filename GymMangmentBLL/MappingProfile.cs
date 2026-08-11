using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using GymMangmentBLL.ViewModels.SessionViewModels;
using GymMangmentDAL.Entities;

namespace GymMangmentBLL
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Session, SessionViewModel>()
             .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.SessionCategory.CategoryName))
             .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.SessionTrainer.Name))
             .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore()); // Will Be Calculated After Map

            CreateMap<CreateSessionViewModel, Session>();

            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
        }
    }
}
