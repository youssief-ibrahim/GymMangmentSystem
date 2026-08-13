using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymMangmentBLL.Services.InterFaces;
using GymMangmentBLL.ViewModels.AnalysisViewModel;
using GymMangmentDAL.Entities;
using GymMangmentDAL.Repositories.Classes;
using GymMangmentDAL.Repositories.interfaces;

namespace GymMangmentBLL.Services.Classes
{
    public class AnalysisService : IAnalysisService
    {
        private readonly IUnitOfWork unitOfWork;

        public AnalysisService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public AnalysisViewModel GetAnalysisData()
        {
            var Sessions = unitOfWork.GetRepository<Session>().GetAll();
            return new AnalysisViewModel
            {
                totalMembers = unitOfWork.GetRepository<Member>().GetAll().Count(),
                ActiveMembers = unitOfWork.GetRepository<MemberShip>().GetAll(ms => ms.Status == "Active").Count(),
                TotalTrainers = unitOfWork.GetRepository<Trainer>().GetAll().Count(),
                UpcomingSessions = Sessions.Count(s => s.StartDate > DateTime.Now),
                OngoingSessions = Sessions.Count(s => s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now),
                CompletedSessions = Sessions.Count(s => s.EndDate < DateTime.Now)
            };
        }
    }
}
