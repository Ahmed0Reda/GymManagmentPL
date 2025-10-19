using GymmanagmentBLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymmanagmentBLL.Services.Interfaces
{
    public interface ITrainerService
    {
        bool CreateTrainer(CreateTrainerViewModel model);
        bool UpdateTrainerDetails(int trainerId, TrainerToUpdateViewModel model);
        bool RemoveTrainer(int trainerId);
        IEnumerable<TrainerViewModel> GetAllTrainers();
        TrainerViewModel? GetTrainerDetails(int trainerId);
        TrainerToUpdateViewModel? GetTrainerForUpdate(int trainerId);
    }
}
