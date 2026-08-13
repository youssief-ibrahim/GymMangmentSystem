using System.Diagnostics;
using GymMangmentPL.Models;
using Microsoft.AspNetCore.Mvc;
using GymMangmentBLL.Services.InterFaces;
namespace GymMangmentPL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnalysisService analysisService;

        public HomeController(IAnalysisService AnalysisService)
        {
            analysisService = AnalysisService;
        }
        public IActionResult Index()
        {
            var Data=analysisService.GetAnalysisData();
            return View(Data);
        }
    }
}
