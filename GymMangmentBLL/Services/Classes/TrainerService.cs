using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymMangmentBLL.Services.InterFaces;
using GymMangmentBLL.ViewModels.TrainerViewModel;
using GymMangmentDAL.Entities;
using GymMangmentDAL.Repositories.Classes;
using GymMangmentDAL.Repositories.interfaces;

namespace GymMangmentBLL.Services.Classes
{
    internal class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public bool CreateTrainer(CreateTrainerViewModel createTrainerViewModel)
        {
            try
            {
                if (IsExistMail(createTrainerViewModel.Email) || IsExistPhone(createTrainerViewModel.Phone)) return false;
                var trainer = new Trainer()
                {
                    Name = createTrainerViewModel.Name,
                    Email = createTrainerViewModel.Email,
                    Phone = createTrainerViewModel.Phone,
                    DateOfBirth = createTrainerViewModel.DateOfBirth,
                    Gender = createTrainerViewModel.Gender,
                    Address = new Address()
                    {
                        BuildingNumber = createTrainerViewModel.BuildingNumber,
                        Street = createTrainerViewModel.Street,
                        City = createTrainerViewModel.City,
                    },
                    Specialist = createTrainerViewModel.Specialties,

                };
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
            return trainer.Select(x => new TrainerViewModels()
            {
                Email = x.Email,
                Name = x.Name,
                Id = x.Id,
                Phone = x.Phone,
                Specialization=x.Specialist.ToString()
            });  
        }

        public TrainerViewModels? GetTrainerDetails(int TrainerId)
        {
            var trainer = unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (trainer is null ) return null;
            return new TrainerViewModels()
            {
                Email = trainer.Email,
                Name = trainer.Name,
                Id = trainer.Id,
                Phone = trainer.Phone,
                Specialization = trainer.Specialist.ToString()
            };
        }

        public TrainerToUpdateViewModel? GetTrainerForUpdate(int TrainerId)
        {

            var trainer = unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (trainer is null ) return null;
         
            var ViewModel = new TrainerToUpdateViewModel()
            {
               Name = trainer.Name,
               Email = trainer.Email,
               Phone = trainer.Phone,
               BuildingNumber=trainer.Address.BuildingNumber,
               Street=trainer.Address.Street,
               City=trainer.Address.City,
               Specialties=trainer.Specialist,
            };
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

                trainer.Email = TrainerToUpdate.Email;
                trainer.Phone = TrainerToUpdate.Phone;
                trainer.Address.BuildingNumber = TrainerToUpdate.BuildingNumber;
                trainer.Address.City = TrainerToUpdate.City;
                trainer.Address.City = TrainerToUpdate.Street;
                trainer.UpdatedAt = DateTime.Now;
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
