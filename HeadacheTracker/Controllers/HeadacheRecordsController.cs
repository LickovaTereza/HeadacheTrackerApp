using System.Data;
using System.Security.Claims;
using System.Text.Json;
using HeadacheTracker.DTO;
using HeadacheTracker.Services;
using HeadacheTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HeadacheTracker.Controllers {
    public class HeadacheRecordsController : Controller {
        HeadacheRecordService _headacheRecordService;
        public HeadacheRecordsController(HeadacheRecordService headacheRecordService) {
            _headacheRecordService = headacheRecordService;
        }
        public async Task<IActionResult> Index(HeadacheRecordFilterDTO filter) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Načtení filtrovaných záznamů
            IEnumerable<HeadacheRecordDTO> filteredHeadacheRecords = await _headacheRecordService.GetAll(userId, filter);

            // Načtení dat pro dropdowny
            HeadacheRecordsDropdownsViewModel dropdownsData = _headacheRecordService.GetHeadacheRecordsDropdownsData(userId);

            // Vytvoření ViewModelu a předání do View
            var viewModel = new HeadacheRecordsViewModel {
                Records = filteredHeadacheRecords,
                Filter = filter,
                Dropdowns = dropdownsData 
            };

            return View(viewModel);
        }

        // GET: HeadacheRecords/Create
        [HttpGet]
        public IActionResult Create() {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            HeadacheRecordDTO model;

            if (TempData["FormData"] != null) {
                var json = TempData["FormData"].ToString();
                model = JsonSerializer.Deserialize<HeadacheRecordDTO>(json);
            }
            else {
                model = new HeadacheRecordDTO();
            }

            FillDropdowns(userId);
            return View(model);
        }

        // POST: HeadacheRecords/Create
        [HttpPost]
        public async Task<IActionResult> CreateAsync(HeadacheRecordDTO newHeadacheRecord) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!ModelState.IsValid) {
                FillDropdowns(userId);
                return View(newHeadacheRecord);
            }

            await _headacheRecordService.CreateAsync(newHeadacheRecord, userId);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            HeadacheRecordDTO model;

            if (TempData["FormData"] != null) {
                var json = TempData["FormData"].ToString();
                model = JsonSerializer.Deserialize<HeadacheRecordDTO>(json);
                model.Id = id; // zachovám správné ID pro editaci
            }
            else {
                model = await _headacheRecordService.FindByIdAsync(id, userId);
                if (model == null) {
                    return NotFound("NotFound");
                }
            }

            FillDropdowns(userId);
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(HeadacheRecordDTO updatedHeadacheRecord) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!ModelState.IsValid) {
                FillDropdowns(userId);
                return View(updatedHeadacheRecord);
            }

            await _headacheRecordService.UpdateAsync(updatedHeadacheRecord, userId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _headacheRecordService.DeleteAsync(id, userId);
            return RedirectToAction("Index");
        }

        private void FillDropdowns(string userId) {
            HeadacheRecordsDropdownsViewModel dropdownsData = _headacheRecordService.GetHeadacheRecordsDropdownsData(userId);
            ViewBag.Medications = new SelectList(dropdownsData.Medications, "Id", "Name");
            ViewBag.Treatments = new SelectList(dropdownsData.Treatments, "Id", "Name");
            ViewBag.Triggers = new SelectList(dropdownsData.Triggers, "Id", "Name");
        }

        // METODY PRO PŘESUN NA CREATE T+T+M
        [HttpPost]
        public IActionResult RedirectToCreateTrigger(HeadacheRecordDTO currentForm) {
            TempData["FormData"] = JsonSerializer.Serialize(currentForm);
            TempData.Keep("FormData");
            return RedirectToAction("Create", "Triggers", new {
                returnUrl = Url.Action("Create", "HeadacheRecords")
            });
        }

        [HttpPost]
        public IActionResult RedirectToEditTrigger(int id, HeadacheRecordDTO currentForm) {
            TempData["FormData"] = JsonSerializer.Serialize(currentForm);
            TempData.Keep("FormData");

            return RedirectToAction("Create", "Triggers", new {
                returnUrl = Url.Action("Edit", "HeadacheRecords", new { id = id })
            });
        }

        [HttpPost]
        public IActionResult RedirectToCreateMedication(HeadacheRecordDTO currentForm) {
            TempData["FormData"] = JsonSerializer.Serialize(currentForm);
            TempData.Keep("FormData");
            return RedirectToAction("Create", "Medications", new {
                returnUrl = Url.Action("Create", "HeadacheRecords")
            });
        }

        [HttpPost]
        public IActionResult RedirectToEditMedication(int id, HeadacheRecordDTO currentForm) {
            TempData["FormData"] = JsonSerializer.Serialize(currentForm);
            TempData.Keep("FormData");

            return RedirectToAction("Create", "Medications", new {
                returnUrl = Url.Action("Edit", "HeadacheRecords", new { id = id })
            });
        }


        [HttpPost]
        public IActionResult RedirectToCreateTreatment(HeadacheRecordDTO currentForm) {
            TempData["FormData"] = JsonSerializer.Serialize(currentForm);
            TempData.Keep("FormData");
            return RedirectToAction("Create", "Treatments", new {
                returnUrl = Url.Action("Create", "HeadacheRecords")
            });
        }

        [HttpPost]
        public IActionResult RedirectToEditTreatment(int id, HeadacheRecordDTO currentForm) {
            TempData["FormData"] = JsonSerializer.Serialize(currentForm);
            TempData.Keep("FormData");

            return RedirectToAction("Create", "Treatments", new {
                returnUrl = Url.Action("Edit", "HeadacheRecords", new { id = id })
            });
        }

    }
}
