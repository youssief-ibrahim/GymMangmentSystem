using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymMangmentBLL.ViewModels.MemberViewModel;
using GymMangmentBLL.ViewModels.PlanViewModel;

namespace GymMangmentBLL.Services.InterFaces
{
    public interface IPlanService
    {
        IEnumerable<PlanViewModel> GetAllPlanss();
        PlanViewModel? GetPlanById(int PlanId);
        UpdatePlanViewModel? GetPlanForUpdate(int PlanId);
        bool UpdatePlan(int PlanId, UpdatePlanViewModel updatePlanViewModel);
        bool TogglePlanStatus(int PlanId);
    }
}
