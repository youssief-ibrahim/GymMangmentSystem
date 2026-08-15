using GymMangmentBLL.Services.InterFaces;
using GymMangmentBLL.ViewModels.TrainerViewModel;
using Microsoft.AspNetCore.Mvc;

namespace GymMangmentPL.Controllers
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
            var trainers = _trainerService.GetAllTrainer();
            return View(trainers);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateTrainer(CreateTrainerViewModel createTrainerView)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Check data and missing field");
                return View("Create", createTrainerView);
            }
            var isCreated = _trainerService.CreateTrainer(createTrainerView);
            if (!isCreated)
            {
                TempData["Error"] = "Something went wrong";
                return View("Create", createTrainerView);
            }
            TempData["Success"] = "Trainer Created Successfully";
            return RedirectToAction("Index");

        }

        public IActionResult TrainerDetails(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Invalid Trainer Id";
                return RedirectToAction("Index");
            }
            var trainer = _trainerService.GetTrainerDetails(id);
            if (trainer is null)
            {
                TempData["Error"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }
        public IActionResult TrainerEdit(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Invalid Trainer Id";
                return RedirectToAction("Index");
            }
            var trainer = _trainerService.GetTrainerForUpdate(id);
            if (trainer is null)
            {
                TempData["Error"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }
        [HttpPost]
        public IActionResult TrainerEdit( int id, TrainerToUpdateViewModel updateTrainerView)
        {
            if (!ModelState.IsValid)
            {
                return View("TrainerEdit", updateTrainerView);
            }
            bool isUpdated = _trainerService.UpdateTrainer(id, updateTrainerView);
            if (!isUpdated)
            {
                TempData["Error"] = "Something went wrong";
                return View("TrainerEdit", updateTrainerView);
            }
            TempData["Success"] = "Trainer Updated Successfully";
            return RedirectToAction("Index");
        }
        public IActionResult DeleteTrainer(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Invalid Trainer Id";
                return RedirectToAction("Index");
            }
            var trainer = _trainerService.GetTrainerDetails(id);

            if (trainer is null)
            {
                TempData["Error"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TrainerId = id;
            return View();
        }
        [HttpPost]
        public IActionResult ConfirmDeleteTrainer(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Invalid Trainer Id";
                return RedirectToAction("Index");
            }
            bool isDeleted = _trainerService.RemoveTrainer(id);
            if (!isDeleted)
            {
                TempData["Error"] = "Something went wrong or Trainer has active sessions";
                return RedirectToAction("DeleteTrainer", new { id = id });
            }
            TempData["Success"] = "Trainer Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
