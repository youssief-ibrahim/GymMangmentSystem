using GymMangmentBLL.Services.Classes;
using GymMangmentBLL.Services.InterFaces;
using Microsoft.AspNetCore.Mvc;

namespace GymMangmentPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService memberService;

        public MemberController(IMemberService memberService)
        {
            this.memberService = memberService;
        }
        public IActionResult Index()
        {
            var data=memberService.GetAllMembers();
            return View(data);
        }
        public IActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Invalid Member Id";
                return RedirectToAction(nameof(Index));
            }
            var member = memberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["Error"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        public IActionResult HealthRecord(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Invalid Member Id";
                return RedirectToAction("Index");
            }
            var healthRecord = memberService.GetMemberHealthRecordDetails(id);
            if (healthRecord is null)
            {
                TempData["Error"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecord);
        }
    }
}
