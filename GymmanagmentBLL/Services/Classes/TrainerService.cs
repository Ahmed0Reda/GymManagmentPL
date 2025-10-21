using GymmanagmentBLL.Services.Interfaces;
using GymmanagmentBLL.ViewModels;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Entities.Enum;
using GymManagmentDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymmanagmentBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool CreateTrainer(CreateTrainerViewModel model)
        {
            try
            {
                if (IsEmailExists(model.Email))
                    return false;
                if (IsPhoneExists(model.Phone))
                    return false;
                var trainer = new Trainer
                {
                    Name = model.Name,
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    Address = new Address
                    {
                        BuildingNumber = model.BuildingNumber,
                        City = model.City,
                        Street = model.Street
                    },
                    Specialities = model.Specialities, 
                };
                _unitOfWork.GetRepository<Trainer>().Add(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll() ?? [];
            if (trainers is null || !trainers.Any())
                return [];
            var trainerViewModels = trainers.Select(x => new TrainerViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Specialities = x.Specialities.ToString()

            });
            return trainerViewModels;
        }

        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer is null)
                return null;
            var trainerViewModel = new TrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                PhoneNumber = trainer.PhoneNumber,
                DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
                Gender = trainer.Gender.ToString(),
                Address = FormatAddress(trainer.Address),
                Specialities = trainer.Specialities.ToString()
            };

            return trainerViewModel;
        }

        public TrainerToUpdateViewModel? GetTrainerForUpdate(int trainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer is null)
                return null;
            var trainerToUpdateViewModel = new TrainerToUpdateViewModel
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.PhoneNumber,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City,
                Specialities = trainer.Specialities
            };
            return trainerToUpdateViewModel;
        }

        public bool RemoveTrainer(int trainerId)
        {
            var trainerRepo = _unitOfWork.GetRepository<Trainer>();
            var trainer = trainerRepo.GetById(trainerId);
            var sessions = _unitOfWork.GetRepository<Session>().GetAll(x => x.TrainerId == trainerId && x.StartDate > DateTime.UtcNow);
            if (trainer is null)
                return false;
            if (sessions is not null && sessions.Any())
                return false;
            trainerRepo.Delete(trainer);
            _unitOfWork.SaveChanges();
            return true;

        }

        public bool UpdateTrainerDetails(int trainerId, TrainerToUpdateViewModel model)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer is null)
                return false;
            if (IsEmailExists(model.Email))
                return false;
            if (IsPhoneExists(model.Phone))
                return false;
            trainer.Name = model.Name;
            trainer.Email = model.Email;
            trainer.PhoneNumber = model.Phone;
            trainer.Address.BuildingNumber = model.BuildingNumber;
            trainer.Address.Street = model.Street;
            trainer.Address.City = model.City;
            trainer.UpdatedAt = DateTime.Now;
            trainer.Specialities = model.Specialities;
            _unitOfWork.GetRepository<Trainer>().Update(trainer);
            _unitOfWork.SaveChanges();
            return true;
        }
        #region Helper Methods
        private string FormatAddress(Address address)
        {
            if (address is null)
                return "N/A";
            return $"{address.BuildingNumber}, {address.Street}, {address.City}";
        }
        private bool IsEmailExists(string email)
        {
            var existingMember = _unitOfWork.GetRepository<Member>().GetAll(x => x.Email == email);
            return existingMember is not null && existingMember.Any();
        }
        private bool IsPhoneExists(string phone)
        {
            var existingMember = _unitOfWork.GetRepository<Member>().GetAll(x => x.PhoneNumber == phone);
            return existingMember is not null && existingMember.Any();
        }
        #endregion
    }
}
