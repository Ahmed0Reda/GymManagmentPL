using GymManagementBLL.ViewModels;
using GymmanagmentBLL.Services.Interfaces;
using GymmanagmentBLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        public IActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();
            return View(trainers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateTrainer(CreateTrainerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Create), model);
            }


            bool result = _trainerService.CreateTrainer(model);

            if (result)
            {
                TempData["SuccessMessage"] = "Trainer created successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Trainer with this email or phone already exists.");
                return RedirectToAction(nameof(Index));
            }

        }

        public IActionResult Details(int id)
        {
            var trainer = _trainerService.GetTrainerDetails(id);

            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }

        public IActionResult Edit(int id)
        {
            var trainer = _trainerService.GetTrainerForUpdate(id);

            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpPost]
        public IActionResult Edit(int id, TrainerToUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = _trainerService.UpdateTrainerDetails(id, model);

            if (result)
            {
                TempData["SuccessMessage"] = "Trainer updated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update trainer.";
            }

            return RedirectToAction(nameof(Index));

        }

        public IActionResult Delete(int id)
        {
            var trainer = _trainerService.GetTrainerDetails(id);

            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TrainerId = trainer.Id;
            return View();
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _trainerService.RemoveTrainer(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Trainer deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete trainer";
            }


            return RedirectToAction(nameof(Index));
        }
    }
}