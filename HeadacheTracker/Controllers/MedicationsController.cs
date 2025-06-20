using HeadacheTracker.DTO;
using HeadacheTracker.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;


namespace HeadacheTracker.Controllers {
    public class MedicationsController : Controller {
        private readonly MedicationService _medicationService;
        public MedicationsController(MedicationService medicationService) {
            _medicationService = medicationService;
        }

        // GET: Medications
        [HttpGet]
        public async Task<IActionResult> Index() { 
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var activeMedications = (await _medicationService.GetAll(userId))
                                        .Where(t => t.Id != -1);

            var medicationsForChart = await _medicationService.GetAll(userId);
            ViewBag.MedicationsForChart = medicationsForChart;

            return View(activeMedications); // tabulka bude zobrazovat jen aktivní
        }

        // GET: Triggers/Create
        [HttpGet]
        public IActionResult Create(string returnUrl, string formDataJson) {
            if (!string.IsNullOrEmpty(formDataJson)) {
                TempData["FormData"] = formDataJson;
                TempData.Keep("FormData"); // zajistí, že data přežijí přesměrování
            }

            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index", "Medications");
            return View();
        }

        // POST: Triggers/Create
        [HttpPost]
        public async Task<IActionResult> CreateAsync(MedicationDTO newMedication, string returnUrl) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!ModelState.IsValid) {
                return View(newMedication);
            }

            await _medicationService.CreateAsync(newMedication, userId);
            if (!string.IsNullOrEmpty(returnUrl)) {
                return Redirect(returnUrl); // přesměruje zpět na formulář HeadacheRecord
            }
            return RedirectToAction("Index");
        }

        // GET: Triggers/Edit/5
        [HttpGet]
        public async Task<IActionResult> EditAsync(int id) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var medicationToEdit = await _medicationService.GetByIdAsync(id, userId);
            if (medicationToEdit == null) {
                return View("NotFound");
            }
            return View(medicationToEdit);
        }

        // POST: Triggers/Edit/5
        [HttpPost]
        public async Task<IActionResult> EditAsync(MedicationDTO medicationDTO, int id) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _medicationService.UpdateAsync(medicationDTO, id, userId);
            return RedirectToAction("Index");
        }

        // POST: SoftDelete
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool deleted = await _medicationService.SoftDeleteAsync(id, userId);

            if (!deleted) {
                TempData["ErrorMessage"] = "Medikament je nyní využíván v některém z historických záznamů.\n\nNejprve jej odstraňte v historii.";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
