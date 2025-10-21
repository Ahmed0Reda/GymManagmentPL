using GymmanagmentBLL.Services.Interfaces;
using GymmanagmentBLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        public IActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        }
        public IActionResult MemberDetails(int id)
        {
            var member = _memberService.GetMemberDetails(id);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        public IActionResult HealthRecordDetails(int id)
        {
            var healthRecord = _memberService.GetMemberHealthRecord(id);
            if (healthRecord == null)
            {
                TempData["ErrorMessage"] = "Member not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecord);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateMember(CreateMemberViewModel input)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Check Missing Data");
                return View(nameof(Create), input);
            }
            bool result = _memberService.CreateMember(input);
            if (result)
                TempData["SuccessMessage"] = "Member Created Successfully!";
            else
                TempData["ErrorMessage"] = "Member Failed to Create, Phone Number or Email Already Exist!";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult MemberEdit(int id)
        {
            var member = _memberService.GetMemberForUpdate(id);
            if(member is null)
            {
                TempData["ErrorMessage"] = "Member not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        [HttpPost]
        public IActionResult MemberEdit([FromRoute]int id, MemberToUpdateViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }
            bool result = _memberService.UpdateMemberDetails(id, input);
            if (result)
                TempData["SuccessMessage"] = "Member Updated Successfully!";
            else
                TempData["ErrorMessage"] = "Member Failed to Update!";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete([FromRoute]int id)
        {
            if(id < 0)
            {
                TempData["ErrorMessage"] = "ID  of Member Can't be Zero or Negative!";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = id;
            return View();
        }
        [HttpPost]
        public IActionResult DeleteConfirmed([FromForm]int id)
        {
            var result = _memberService.RemoveMember(id);
            if (result)
                TempData["SuccessMessage"] = "Member Deleted Successfully!";
            else
                TempData["ErrorMessage"] = "Member Can't be Delted!";
            return RedirectToAction(nameof(Index));
        }
    }
}
