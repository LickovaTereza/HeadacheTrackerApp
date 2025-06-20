using HeadacheTracker.DTO;
using HeadacheTracker.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks; 


namespace HeadacheTracker.Controllers {
    public class TreatmentsController : Controller {
        private TreatmentService _treatmentService;
        public TreatmentsController(TreatmentService treatmentService) {
            _treatmentService = treatmentService;
        }

        // GET: Treatments
        [HttpGet]
        public async Task<IActionResult> Index() {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var activeTreatments = (await _treatmentService.GetAll(userId))
                                        .Where(t => t.Id != -1);

            var treatmentsForChart = await _treatmentService.GetAll(userId);
            ViewBag.TreatmentsForChart = treatmentsForChart;

            return View(activeTreatments); // tabulka bude zobrazovat jen aktivní
        }

        // GET: Treatments/Create
        [HttpGet]
        public IActionResult Create(string returnUrl, string formDataJson) {
            if (!string.IsNullOrEmpty(formDataJson)) {
                TempData["FormData"] = formDataJson;
                TempData.Keep("FormData"); // zajistí, že data přežijí přesměrování
            }

            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index", "Treatments");
            return View();
        }

        // POST: Treatments/Create
        [HttpPost]
        public async Task<IActionResult> CreateAsync(TreatmentDTO newTreatment, string returnUrl) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!ModelState.IsValid) {
                return View(newTreatment);
            }

            await _treatmentService.CreateAsync(newTreatment, userId);
            if (!string.IsNullOrEmpty(returnUrl)) {
                return Redirect(returnUrl); // přesměruje zpět na formulář HeadacheRecord
            }
            return RedirectToAction("Index");
        }

        // GET: Treatments/Edit/5
        [HttpGet]
        public async Task<IActionResult> EditAsync(int id) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var treatmentToEdit = await _treatmentService.GetByIdAsync(id, userId);
            if (treatmentToEdit == null) {
                return View("NotFound");
            }
            return View(treatmentToEdit);
        }

        // POST: Treatments/Edit/5
        [HttpPost]
        public async Task<IActionResult> EditAsync(TreatmentDTO treatmentDTO, int id) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _treatmentService.UpdateAsync(treatmentDTO, id, userId);
            return RedirectToAction("Index");
        }

        // POST: SoftDelete
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool deleted = await _treatmentService.SoftDeleteAsync(id, userId);

            if (!deleted) {
                TempData["ErrorMessage"] = "Léčebný způsob je nyní využíván v některém z historických záznamů.\n\nNejprve jej odstraňte v historii.";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
