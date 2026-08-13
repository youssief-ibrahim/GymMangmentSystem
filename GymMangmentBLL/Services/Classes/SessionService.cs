using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using GymMangmentBLL.Services.InterFaces;
using GymMangmentBLL.ViewModels.SessionViewModels;
using GymMangmentDAL.Entities;
using GymMangmentDAL.Repositories.Classes;
using GymMangmentDAL.Repositories.interfaces;

namespace GymMangmentBLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SessionService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public bool CreatedSession(CreateSessionViewModel CreatedSession)
        {
            try
            {
                if (IsTaranerExisst(CreatedSession.TrainerId)) return false;
                if (IsCategoryExisst(CreatedSession.CategoryId)) return false;
                if (IsTimeValid(CreatedSession.StartDate, CreatedSession.EndDate)) return false;
                if (CreatedSession.Capacity < 0 || CreatedSession.Capacity > 25) return false;

                var sessionadd = mapper.Map<Session>(CreatedSession);
                unitOfWork.GetRepository<Session>().Add(sessionadd);
                return unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eror is Exist {ex}");
                return false;
            }
           
        }

        public IEnumerable<SessionViewModel> GeTAllSession()
        {
            var session=unitOfWork.SessionRepository.GetAllSessionWithTrainerAndCategory();
            if (!session.Any()) return [];

            //return session.Select(s => new SessionViewModel()
            //{
            //    Id = s.Id,
            //    Capacity = s.Capacity,
            //    StartDate = s.StartDate,
            //    EndDate = s.EndDate,
            //    Description = s.Description,
            //    TrainerName=s.SessionTrainer.Name,
            //    AvailableSlots=s.Capacity - unitOfWork.SessionRepository.GetCountOfBookedSloute(s.Id),
            //    CategoryName=s.SessionCategory.CategoryName,

            //});
            var mappingSeession = mapper.Map<IEnumerable<Session>,IEnumerable<SessionViewModel>>(session);
            foreach (var sessionItem in mappingSeession)
            {
                sessionItem.AvailableSlots = sessionItem.Capacity - unitOfWork.SessionRepository.GetCountOfBookedSloute(sessionItem.Id);
            }
            return mappingSeession;
        }

        public SessionViewModel? GetSessionById(int SessionId)
        {
            var session = unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(SessionId);
            if (session is null ) return null;

            var mappingSeession = mapper.Map<Session, SessionViewModel>(session);

            mappingSeession.AvailableSlots = mappingSeession.Capacity - unitOfWork.SessionRepository.GetCountOfBookedSloute(mappingSeession.Id);

            return mappingSeession;
        }

        public UpdateSessionViewModel? GetSessionForUpdate(int id)
        {
            var session = unitOfWork.SessionRepository.GetById(id);
            if (!IsSessionAvilableToUpdata(session!))
            {
                return null;
            }
            var MappedSession = mapper.Map<UpdateSessionViewModel>(session);
            return MappedSession;
        }

        public bool UpdateSession(int id, UpdateSessionViewModel updateSessionViewModel)
        {
            try
            {
                var session = unitOfWork.SessionRepository.GetById(id);
                if (!IsSessionAvilableToUpdata(session!))
                {
                    return false;
                }
                if (!IsTaranerExisst(updateSessionViewModel.TrainerId))
                {
                    return false;
                }
                if (!IsTimeValid(updateSessionViewModel.StartDate, updateSessionViewModel.EndDate))
                {
                    return false;
                }
                mapper.Map(updateSessionViewModel, session);
                session!.UpdatedAt = DateTime.Now;
                unitOfWork.GetRepository<Session>().Update(session);
                return unitOfWork.SaveChanges() > 0;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteSession(int id)
        {
            try
            {
                var session = unitOfWork.SessionRepository.GetById(id);
                if (!IsSessionAvilableToDelete(session!))
                {
                    return false;
                }
                unitOfWork.GetRepository<Session>().Delete(session!);
                return unitOfWork.SaveChanges()>0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eror is {ex}");
                return false;
            }
            
        }


        #region Helper-Method

        private bool IsSessionAvilableToUpdata(Session session)
        {
            if (session is null) return false;

            if (session.EndDate < DateTime.Now)
            {
                return false;
            }

            if (session.StartDate <= DateTime.Now)
            {
                return false;
            }
            var HasActiveBooking = unitOfWork.SessionRepository.GetCountOfBookedSloute(session.Id) > 0;

            if (HasActiveBooking) return false;

            return true;
        }
        private bool IsSessionAvilableToDelete(Session session)
        {
            if (session is null) return false;

            if (session.EndDate <= DateTime.Now && session.EndDate>DateTime.Now)
            {
                return false;
            }
            // uocoming
            if (session.StartDate > DateTime.Now)
            {
                return false;
            }
            var HasActiveBooking = unitOfWork.SessionRepository.GetCountOfBookedSloute(session.Id) > 0;

            if (HasActiveBooking) return false;

            return true;
        }

        private bool IsTaranerExisst(int TrainerId)
        {
            return unitOfWork.GetRepository<Session>().GetById(TrainerId) is not null;
        }
        private bool IsCategoryExisst(int CategoryId)
        {
            return unitOfWork.GetRepository<Session>().GetById(CategoryId) is not null;
        }
        private bool IsTimeValid(DateTime startDate, DateTime endDate)
        {
            return startDate < endDate;
        }


        #endregion
    }
}
