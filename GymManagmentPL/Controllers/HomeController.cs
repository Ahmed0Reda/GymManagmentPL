using System.Diagnostics;
using GymmanagmentBLL.Services.Interfaces;
using GymManagmentPL.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnalyticsService _analyticsService;

        public HomeController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }
        public IActionResult Index()
        {
            var analytics = _analyticsService.GetAnalyticsData();
            return View(analytics);
        }
    }
}
