using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using GymMangmentBLL.ViewModels.MemberViewModel;
using GymMangmentBLL.ViewModels.PlanViewModel;
using GymMangmentBLL.ViewModels.SessionViewModels;
using GymMangmentBLL.ViewModels.TrainerViewModel;
using GymMangmentDAL.Entities;

namespace GymMangmentBLL
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            MapingSession();
            MapMember();
            MapTrainer();
            MapPlan();
        }
        private void MapingSession()
        {
            CreateMap<Session, SessionViewModel>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.SessionCategory.CategoryName))
            .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.SessionTrainer.Name))
             .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore()); // Will Be Calculated After Map

            CreateMap<CreateSessionViewModel, Session>();

            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
            
            CreateMap<Trainer,TrainerSelectViewModel>();
            CreateMap<Category,CategorySelectViewModel>();
        }
        private void MapMember()
        {
            // make this if you dont use  CreateMap<CreateMemberViewModel, Member>() any where else
            //CreateMap<CreateMemberViewModel, Member>()
            //.ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address()
            //{
            //    BuildingNumber = src.BuildingNumber,
            //    City = src.City,
            //    Street = src.Street,
            //}));
            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel))
                 .ForMember(dest => dest.Photo, opt => opt.Ignore());

            CreateMap<CreateMemberViewModel, Address>()
            .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street));

            CreateMap<HealthRecordViewModel, HealthRecord>().ReverseMap();

            CreateMap<Member, MemberViewModels>()
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));

            CreateMap<Member, MemberToUpdateViewModel>()
            .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street));

            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.Address.City = src.City;
                    dest.Address.Street = src.Street;
                    dest.UpdatedAt = DateTime.Now;
                });

        }
        private void MapTrainer()
        {
            CreateMap<CreateTrainerViewModel, Trainer>()
              .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address()
              {
                  BuildingNumber = src.BuildingNumber,
                  City = src.City,
                  Street = src.Street,
              }))
              .ForMember(dest => dest.Specialist, opt => opt.MapFrom(src => src.Specialties));

            CreateMap<Trainer, TrainerViewModels>()
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src =>src.Specialist.ToString()));

            CreateMap<Trainer, TrainerToUpdateViewModel>()
           .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
           .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
           .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
           .ForMember(dest => dest.Specialties, opt => opt.MapFrom(src => src.Specialist));

            
              CreateMap<TrainerToUpdateViewModel, Trainer>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.Address.City = src.City;
                    dest.Address.Street = src.Street;
                    dest.Specialist=src.Specialties;
                    dest.UpdatedAt = DateTime.Now;
                });
        }
        private void MapPlan()
        {
            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan, UpdatePlanViewModel>().ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Name));
            CreateMap<UpdatePlanViewModel, Plan>()
           .ForMember(dest => dest.Name, opt => opt.Ignore())
           .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

        }
    }
}
