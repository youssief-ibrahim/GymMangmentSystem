using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GymMangmentBLL.Services.InterFaces;
using GymMangmentBLL.ViewModels.PlanViewModel;
using GymMangmentDAL.Entities;
using GymMangmentDAL.Repositories.interfaces;

namespace GymMangmentBLL.Services.Classes
{
    internal class PlanService : IPlanService
    {
        private readonly IUnitOfWork unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<PlanViewModel> GetAllMembers()
        {
            var plans=unitOfWork.GetRepository<Plan>().GetAll();
            if (plans == null || !plans.Any()) return [];

            var planVM = plans.Select(e => new PlanViewModel
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                DurationInDays = e.DurationInDays,
                IsActive = e.IsActive,
                Price=e.Price,
            });
            return planVM;
        }

        public PlanViewModel? GetPlanById(int PlanId)
        {
            var plan = unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null) return null;

            var planVM = new PlanViewModel()
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                DurationInDays = plan.DurationInDays,
                IsActive = plan.IsActive,
                Price = plan.Price,
            };
            return planVM;
        }

        public UpdatePlanViewModel? GetPlanForUpdate(int PlanId)
        {
            var plan = unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null || plan.IsActive==false || HasActivemembershipr((PlanId))) return null;

            return new UpdatePlanViewModel()
            {
                Description = plan.Description,
                DurationDays = plan.DurationInDays,
                PlanName = plan.Name,
                Price = plan.Price,
            };

        }
        public bool UpdatePlan(int PlanId, UpdatePlanViewModel updatePlanViewModel)
        {
            var plan = unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null || HasActivemembershipr((PlanId))) return false;
            try
            {
                (plan.Description, plan.Price, plan.DurationInDays, plan.UpdatedAt) =
                (updatePlanViewModel.Description, updatePlanViewModel.Price, updatePlanViewModel.DurationDays, DateTime.Now);

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
