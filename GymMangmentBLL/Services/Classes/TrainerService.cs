using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymMangmentBLL.Services.InterFaces;
using GymMangmentBLL.ViewModels.TrainerViewModel;
using GymMangmentDAL.Entities;
using GymMangmentDAL.Repositories.Classes;
using GymMangmentDAL.Repositories.interfaces;

namespace GymMangmentBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TrainerService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public bool CreateTrainer(CreateTrainerViewModel createTrainerViewModel)
        {
            try
            {
                if (IsExistMail(createTrainerViewModel.Email) || IsExistPhone(createTrainerViewModel.Phone)) return false;
                var trainer = mapper.Map<Trainer>(createTrainerViewModel);
                unitOfWork.GetRepository<Trainer>().Add(trainer);
                return unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }

        public IEnumerable<TrainerViewModels> GetAllTrainer()
        {
            var trainer = unitOfWork.GetRepository<Trainer>().GetAll();
            if (trainer is null || !trainer.Any()) return [];
            return mapper.Map< IEnumerable<TrainerViewModels>>(trainer);
        }

        public TrainerViewModels? GetTrainerDetails(int TrainerId)
        {
            var trainer = unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (trainer is null ) return null;
            return mapper.Map<TrainerViewModels>(trainer);
        }

        public TrainerToUpdateViewModel? GetTrainerForUpdate(int TrainerId)
        {

            var trainer = unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (trainer is null ) return null;
         
            var ViewModel =mapper.Map<TrainerToUpdateViewModel>(trainer);
            return ViewModel;
        }

        public bool RemoveTrainer(int TrainerId)
        {
            var trainerRepo = unitOfWork.GetRepository<Trainer>();
            var trainer = trainerRepo.GetById(TrainerId);
            if (trainer is null || HasActiveSessions(TrainerId)) return false;
            try
            {
                trainerRepo.Delete(trainer);
                return unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool UpdateTrainer(int TrainerId, TrainerToUpdateViewModel TrainerToUpdate)
        {
            try
            {
                var trainerRepo = unitOfWork.GetRepository<Trainer>();
                var trainer = trainerRepo.GetById(TrainerId);
                if (trainer is null || IsExistMail(trainer.Email) || IsExistPhone(trainer.Phone)) return false;
                mapper.Map( TrainerToUpdate, trainer);
                trainerRepo.Update(trainer);
                return unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex) { 
                return false;
            }
        }

        #region private-method
        private bool IsExistMail(string mail)
        {
            return unitOfWork.GetRepository<Trainer>().GetAll(e => e.Email == mail).Any();
        }
        private bool IsExistPhone(string phone)
        {
            return unitOfWork.GetRepository<Trainer>().GetAll(e => e.Phone == phone).Any();
        }
        private bool HasActiveSessions(int Id)
        {
            return unitOfWork.GetRepository<Session>()
              .GetAll(X => X.TrainerId == Id &&
                           X.StartDate > DateTime.Now).Any();
        }
        #endregion
    }
}
