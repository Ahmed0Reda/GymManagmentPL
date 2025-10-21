using GymManagementSystemBLL.ViewModels;
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
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public AnalyticsViewModel GetAnalyticsData()
        {
            return new AnalyticsViewModel
            {
                ActiveMembers = _unitOfWork.GetRepository<Membership>().GetAll(x => x.Status == "Active").Count(),
                TotalMembers = _unitOfWork.GetRepository<Member>().GetAll().Count(),
                TotalTrainers = _unitOfWork.GetRepository<Trainer>().GetAll().Count(),
                UpcomingSessions = _unitOfWork.GetRepository<Session>().GetAll(x => x.StartDate > DateTime.Now).Count(),
                OngoingSessions = _unitOfWork.GetRepository<Session>().GetAll(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.UtcNow).Count(),
                CompletedSessions = _unitOfWork.GetRepository<Session>().GetAll(x => x.EndDate < DateTime.Now).Count()
            };
        }
    }
}
