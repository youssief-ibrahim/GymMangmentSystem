using System.Data;
using System;
using GymMangmentBLL.Services.Classes;
using GymMangmentBLL.Services.InterFaces;
using GymMangmentBLL.ViewModels.MemberViewModel;
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
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateMember(CreateMemberViewModel createMember)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Check data and missing field");
                return View(nameof(Create), createMember);
            }
            bool result = memberService.CreateMember(createMember);
            if (result)
            {
                TempData["Success"] = "Member Created Successfully";
            }
            else
            {
                TempData["Error"] = "Failed to Create Member";
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult MemberEdit(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Invalid Member Id";
                return RedirectToAction("Index");
            }
            var member = memberService.GetMemberForUpdate(id);
            if (member is null)
            {
                TempData["Error"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        [HttpPost]
        public ActionResult MemberEdit([FromRoute] int id, MemberToUpdateViewModel memberToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return View(memberToUpdate);
            }
            bool result = memberService.UpdateMember(id, memberToUpdate);
            if (result)
            {
                TempData["Success"] = "Member Updated Successfully";
            }
            else
            {
                TempData["Error"] = "Failed to Update Member";
            }
            return RedirectToAction(nameof(Index));

        }

        public ActionResult MemberDelete(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Invalid Member Id";
                return RedirectToAction("Index");
            }
            var member = memberService.GetMemberDetails(id);

            if (member is null)
            {
                TempData["Error"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.MemberId = id;
            return View();
        }
        [HttpPost]
        public ActionResult DeleteConfig([FromForm] int id)
        {
            var resultMember = memberService.RemoveMember(id);
            if (resultMember)
            {
                TempData["Success"] = "Member Deleted Successfully";
            }
            else
            {
                TempData["Error"] = "Failed to Delete Member";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
