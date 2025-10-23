using AutoMapper;
using GymManagmentBLL.ViewModels;
using GymmanagmentBLL.Services.Interfaces;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymmanagmentBLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel input)
        {
            if(!IsTrainerExists(input.TrainerId))
                return false;
            if(!IsCategoryExists(input.CategoryId))
                return false;
            if(!IsValidDateRange(input.StartDate, input.EndDate))
                return false;
            var session = _mapper.Map<CreateSessionViewModel, Session>(input);
            _unitOfWork.GetRepository<Session>().Add(session);
            return _unitOfWork.SaveChanges() > 0;
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var sessions = _unitOfWork.SessionRepository.GetSessionsWithTrainerAndCategory();
            if (sessions == null || !sessions.Any())
                return [];
            var mappedSessions = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);
            foreach (var session in mappedSessions)
            {
                var bookedSlots = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);
                session.AvailableSlots = session.Capacity - bookedSlots;
            }
            return mappedSessions;
        }

        public SessionViewModel? GetSessionById(int id)
        {
            var session = _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(id);
            if (session == null)
                return null;
            var mappedSession = _mapper.Map<Session, SessionViewModel>(session);
            mappedSession.AvailableSlots = mappedSession.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(mappedSession.Id);
            return mappedSession;

        }
        public UpdateSessionViewModel? GetSessionToUpdate(int id)
        {
            var session = _unitOfWork.GetRepository<Session>().GetById(id);
            if (session == null)
                return null;
            return _mapper.Map<Session, UpdateSessionViewModel>(session);
        }
        public bool UpdateSession(int id, UpdateSessionViewModel input)
        {
            var session = _unitOfWork.GetRepository<Session>().GetById(id);
            if (!IsSessionAvilableForUpdate(session))
                return false;
            if (!IsTrainerExists(input.TrainerId))
                return false;
            if (!IsValidDateRange(input.StartDate, input.EndDate))
                return false;
            session.TrainerId = session.TrainerId;
            session.Description = input.Description;
            session.StartDate = input.StartDate;
            session.EndDate = input.EndDate;
            session.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.GetRepository<Session>().Update(session);
            return _unitOfWork.SaveChanges() > 0;
        }
        public bool RemoveSession(int id)
        {
            var session = _unitOfWork.GetRepository<Session>().GetById(id);
            if (!IsSessionAvilableForRemove(session))
                return false;
            _unitOfWork.GetRepository<Session>().Delete(session);
            return _unitOfWork.SaveChanges() > 0;
        }

        public IEnumerable<CategorySelectViewModel> GetCategoriesDropDown()
        {
            var categories = _unitOfWork.GetRepository<Category>().GetAll();
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(categories);
        }

        public IEnumerable<TrainerSelectViewModel> GetTrainersDropDown()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }

        #region Helper Methods
        private bool IsTrainerExists(int trainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            return trainer is null ? false : true;
        }
        private bool IsCategoryExists(int categoryId)
        {
            var category = _unitOfWork.GetRepository<Category>().GetById(categoryId);
            return category is null ? false : true;
        }
        private bool IsValidDateRange(DateTime startTime, DateTime endTime)
        {
            return startTime < endTime && startTime > DateTime.UtcNow;
        }
        private bool IsSessionAvilableForUpdate(Session session)
        {
            if(session is null)
                return false;
            if(session.EndDate < DateTime.UtcNow)
                return false;
            if (session.StartDate <= DateTime.UtcNow)
                return false;
            var hasActiveBookings = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (hasActiveBookings)
                return false;
            return true;
        }
        private bool IsSessionAvilableForRemove(Session session)
        {
            if(session is null)
                return false;
            if(session.StartDate > DateTime.UtcNow)
                return false;
            if (session.StartDate <= DateTime.UtcNow && session.EndDate > DateTime.UtcNow)
                return false;
            var hasActiveBookings = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (hasActiveBookings)
                return false;
            return true;
        }

        #endregion
    }
}
