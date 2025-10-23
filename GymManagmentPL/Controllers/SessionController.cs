using GymmanagmentBLL.Services.Classes;
using GymmanagmentBLL.Services.Interfaces;
using GymManagmentBLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagmentPL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public IActionResult Index()
        {
            var sessions = _sessionService.GetAllSessions();
            return View(sessions);
        }
        public IActionResult Create()
        {
            LoadCategoriesDropDown();
            LoadTrainersDropDown();
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateSessionViewModel input)
        {
            if (!ModelState.IsValid)
            {
                LoadTrainersDropDown();
                LoadCategoriesDropDown();
                return View(input);
            }
            var result = _sessionService.CreateSession(input);
            if(result)
            {
                TempData["SuccessMessage"] = "Session created successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Failed to create session. Please check the input data.");
                LoadTrainersDropDown();
                LoadCategoriesDropDown();
                return View(input);
            }
        }

        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid session ID.";
                return RedirectToAction("Index");
            }
            var session = _sessionService.GetSessionById(id);
            if(session == null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction("Index");
            }
            return View(session);
        }
        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid session ID.";
                return RedirectToAction("Index");
            }
            var session = _sessionService.GetSessionToUpdate(id);
            if(session == null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction("Index");
            }
            LoadTrainersDropDown();
            return View(session);
        }
        [HttpPost]
        public IActionResult Edit([FromRoute]int id,UpdateSessionViewModel input)
        {
            if (!ModelState.IsValid)
            {
                LoadTrainersDropDown();
                return View(input);
            }
            var result = _sessionService.UpdateSession(id ,input);
            if(result)
            {
                TempData["SuccessMessage"] = "Session updated successfully.";
            }
            else
            {
                ModelState.AddModelError("", "Failed to update session. Please check the input data.");
                return View(input);
            }
            return RedirectToAction("Index");

        }
        public IActionResult Delete([FromRoute] int id)
        {
            if (id < 0)
            {
                TempData["ErrorMessage"] = "ID  of Session Can't be Zero or Negative!";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionById(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SessionId = id;
            return View();
        }
        [HttpPost]
        public IActionResult DeleteConfirmed([FromForm] int id)
        {
            var result = _sessionService.RemoveSession(id);
            if (result)
                TempData["SuccessMessage"] = "Session Deleted Successfully!";
            else
                TempData["ErrorMessage"] = "Session Can't be Delted!";
            return RedirectToAction(nameof(Index));
        }
        #region Helper Methods
        private void LoadCategoriesDropDown()
        {
            var categories = _sessionService.GetCategoriesDropDown();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
        }
        private void LoadTrainersDropDown()
        {
            var trainers = _sessionService.GetTrainersDropDown();
            ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
        }
        #endregion
    }
}
