using GymManagementBLL.ViewModels.PlanViewModels;
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
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool Activate(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null || HasActiveMemberShips(planId))
                return false;
            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.GetRepository<Plan>().Update(plan);
            return _unitOfWork.SaveChanges() > 0;
        }

        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (plans is null || !plans.Any())
                return [];
            return plans.Select(x => new PlanViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                DurationDays = x.DurationDays,
                IsActive = x.IsActive
            });
        }

        public PlanViewModel? GetPlanById(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null)
                return null;
            return new PlanViewModel
                {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                Price = plan.Price,
                DurationDays = plan.DurationDays,
                IsActive = plan.IsActive
            };
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null || plan.IsActive == false)
                return null;
            return new UpdatePlanViewModel
            {
                PlanName = plan.Name,
                Description = plan.Description,
                Price = plan.Price,
                DurationDays = plan.DurationDays
            };
        }

        public bool UpdatePlan(int planId, UpdatePlanViewModel input)
        {
            try
            {
                var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
                if (plan is null || HasActiveMemberShips(planId))
                    return false;
                plan.Name = input.PlanName;
                plan.Description = input.Description;
                plan.Price = input.Price;
                plan.DurationDays = input.DurationDays;
                _unitOfWork.GetRepository<Plan>().Update(plan);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch
            {
                return false;
            }
        }

        #region Helper Methods
        private bool HasActiveMemberShips(int planId)
        {
            return _unitOfWork.GetRepository<Membership>().GetAll(m => m.planId == planId && m.Status == "Active").Any();
            
        }
        #endregion 
    }
}
