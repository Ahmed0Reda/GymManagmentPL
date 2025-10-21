using GymManagementSystemBLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymmanagmentBLL.Services.Interfaces
{
    public interface IAnalyticsService
    {
        AnalyticsViewModel GetAnalyticsData();
    }
}
