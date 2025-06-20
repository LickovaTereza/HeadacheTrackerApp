using HeadacheTracker.DTO;
using HeadacheTracker.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;


namespace HeadacheTracker.Controllers {
    public class TriggersController : Controller {
        private TriggerService _triggerService;
        public TriggersController(TriggerService triggerService) {
            _triggerService = triggerService;
        }

        // GET: Trigger
        [HttpGet]
        public async Task<IActionResult> Index() {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Triggery pro výpis v tabulce – bez "Žádný trigger" - v grafu ho chci
            var activeTriggers = (await _triggerService.GetAll(userId))
                                        .Where(t => t.Id != -1);

            // Pro graf
            var triggersForChart = await _triggerService.GetAll(userId);

            // Předání dat do view
            ViewBag.TriggersForChart = triggersForChart;

            return View(activeTriggers);
        }

        // GET: Triggers/Create
        [HttpGet]
        public IActionResult Create(string returnUrl, string formDataJson) {
            if (!string.IsNullOrEmpty(formDataJson)) {
                TempData["FormData"] = formDataJson;
                TempData.Keep("FormData"); // zajistí, že data přežijí přesměrování
            }

            ViewBag.ReturnUrl = returnUrl; //?? Url.Action("Index", "Triggers");
            return View();
        }


        // POST: Triggers/Create
        [HttpPost]
        public async Task<IActionResult> CreateAsync(TriggerDTO newTrigger, string returnUrl) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _triggerService.CreateAsync(newTrigger, userId);
            if (!string.IsNullOrEmpty(returnUrl)) {
                return Redirect(returnUrl); // přesměruje zpět na formulář HeadacheRecord
            }
            return RedirectToAction("Index");
        }

        // GET: Triggers/Edit/5
        [HttpGet]
        public async Task<IActionResult> EditAsync(int id) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var triggerToEdit = await _triggerService.GetByIdAsync(id, userId);
            if (triggerToEdit == null) {
                return View("NotFound");
            }
            return View(triggerToEdit);
        }

        // POST: Triggers/Edit/5
        [HttpPost]
        public async Task<IActionResult> EditAsync(TriggerDTO triggerDTO, int id) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _triggerService.UpdateAsync(triggerDTO, id, userId);
            return RedirectToAction("Index");
        }

        // POST: SoftDelete
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool deleted = await _triggerService.SoftDeleteAsync(id, userId);

            if (!deleted) {
                TempData["ErrorMessage"] = "Spouštěč je nyní využíván v některém z historických záznamů.\n\nNejprve jej odstraňte v historii.";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
