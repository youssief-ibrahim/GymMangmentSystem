using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymMangmentBLL.ViewModels.MemberViewModel;
using GymMangmentBLL.ViewModels.TrainerViewModel;

namespace GymMangmentBLL.Services.InterFaces
{
    public interface ITrainerService
    {
        IEnumerable<TrainerViewModels> GetAllTrainer();
        bool CreateTrainer(CreateTrainerViewModel createTrainerViewModel);
        TrainerViewModels? GetTrainerDetails(int TrainerId);
        TrainerToUpdateViewModel? GetTrainerForUpdate(int TrainerId);
        bool UpdateTrainer(int TrainerId, TrainerToUpdateViewModel TrainerToUpdate);
        bool RemoveTrainer(int TrainerId);
    }
}
