using System.Diagnostics;
using HeadacheTracker.Models;
using HeadacheTracker.Services;
using HeadacheTracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HeadacheTracker.Controllers {

    [AllowAnonymous]

    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly HeadacheRecordService _headacheRecordService; // prro výpis dnù bez bolesti

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, HeadacheRecordService headacheRecordService) {
            _userManager = userManager;
            _logger = logger;
            _headacheRecordService = headacheRecordService;
        }

        public async Task<IActionResult> Index() {
            var user = await _userManager.GetUserAsync(User);
            int daysWithoutHeadache = -1;

            int withMedication = 0;
            int withoutMedication = 0;
            double percentage = 0.0;

            string mostFrequentTrigger = "Žádný spouštìè";

            string topTriggerName = "Žádný spouštìè";
            int topTriggerCount = 0;
            int otherTriggersCount = 0;

            if (user != null) {
                daysWithoutHeadache = await _headacheRecordService.GetDaysSinceLastHeadacheRecord(user.Id);

                var medicationSummary = await _headacheRecordService.GetMedicationUsageSummary(user.Id);
                withMedication = medicationSummary.WithMedication;
                withoutMedication = medicationSummary.WithoutMedication;
                percentage = medicationSummary.UsagePercentage;

                mostFrequentTrigger = await _headacheRecordService.GetMostFrequentTriggerNameAsync(user.Id);

                var triggerStats = await _headacheRecordService.GetTopTriggerRatioAsync(user.Id);
                topTriggerName = triggerStats.TriggerName;
                topTriggerCount = triggerStats.Count;
                otherTriggersCount = triggerStats.OtherCount;
            }

            var model = new HomePageViewModel {
                DaysWithoutHeadache = daysWithoutHeadache,
                MedicationWithUsage = withMedication,
                MedicationWithoutUsage = withoutMedication,
                UsagePercentage = percentage,
                MostFrequentTrigger = mostFrequentTrigger,
                TopTriggerName = topTriggerName,
                TopTriggerCount = topTriggerCount,
                OtherTriggersCount = otherTriggersCount
            };

            return View(model);
        }

        public IActionResult Information() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
