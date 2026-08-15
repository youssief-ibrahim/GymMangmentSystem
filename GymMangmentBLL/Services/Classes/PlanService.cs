using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymMangmentBLL.Services.InterFaces;
using GymMangmentBLL.ViewModels.PlanViewModel;
using GymMangmentDAL.Entities;
using GymMangmentDAL.Repositories.interfaces;

namespace GymMangmentBLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public PlanService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public IEnumerable<PlanViewModel> GetAllPlanss()
        {
            var plans=unitOfWork.GetRepository<Plan>().GetAll();
            if (plans == null || !plans.Any()) return [];

            var planVM =mapper.Map<IEnumerable<PlanViewModel>>(plans);
            return planVM;
        }

        public PlanViewModel? GetPlanById(int PlanId)
        {
            var plan = unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null) return null;

            var planVM = mapper.Map<PlanViewModel>(plan);
            return planVM;
        }

        public UpdatePlanViewModel? GetPlanForUpdate(int PlanId)
        {
            var plan = unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null || plan.IsActive==false || HasActivemembershipr((PlanId))) return null;

            return mapper.Map<UpdatePlanViewModel>(plan);

        }
        public bool UpdatePlan(int PlanId, UpdatePlanViewModel updatePlanViewModel)
        {
            var plan = unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null || HasActivemembershipr((PlanId))) return false;
            try
            {
                mapper.Map(updatePlanViewModel, plan);
                unitOfWork.GetRepository<Plan>().Update(plan);
                return unitOfWork.SaveChanges()>0;
            }
            catch (Exception ex) { 
                return false;
            }
        }
        public bool TogglePlanStatus(int PlanId)
        {
            var plan = unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null || HasActivemembershipr((PlanId))) return false;

            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.Now;
            try
            {
                unitOfWork.GetRepository<Plan>().Update(plan);
                return unitOfWork.SaveChanges()>0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

       

        private bool HasActivemembershipr(int PlanId)
        {
           return unitOfWork.GetRepository<MemberShip>().GetAll(e=>e.PlanId==PlanId && e.Status== "Active").Any();
        }
    }
}
