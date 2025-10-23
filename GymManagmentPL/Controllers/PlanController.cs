﻿using GymmanagmentBLL.Services.Interfaces;
using GymManagmentBLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }
        public IActionResult Index()
        {
            var plans = _planService.GetAllPlans();
            return View(plans);
        }
        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Plans Can't be 0 or Negative!";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanById(id);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Plans Can't be 0 or Negative!";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanToUpdate(id);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        [HttpPost]
        public IActionResult Edit([FromRoute] int id, UpdatePlanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Data Validation");
                return View(nameof(Edit), model);
            }
            var result = _planService.UpdatePlan(id, model);
            if (result)
            {
                TempData["SuccessMessage"] = "Plan updated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Plan Can't be updated!";
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Activate(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Plans Can't be 0 or Negative!";
                return RedirectToAction(nameof(Index));
            }
            var result = _planService.Activate(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Plan activated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Plan Can't be activated!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
