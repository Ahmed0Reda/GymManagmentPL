using GymManagmentBLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymmanagmentBLL.Services.Interfaces
{
    public interface IPlanService
    {
        bool UpdatePlan(int planId, UpdatePlanViewModel input);
        UpdatePlanViewModel? GetPlanToUpdate(int planId);
        IEnumerable<PlanViewModel> GetAllPlans();
        PlanViewModel? GetPlanById(int planId);
        bool Activate(int planId);
    }
}
