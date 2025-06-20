using System.Diagnostics;
using HeadacheTracker.DTO;
using HeadacheTracker.Models;
using HeadacheTracker.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HeadacheTracker.Services {
    public class HeadacheRecordService {
        AppDbContext _dbContext;
        public HeadacheRecordService(AppDbContext dbContext) {
            _dbContext = dbContext;
        }

        public HeadacheRecordsDropdownsViewModel GetHeadacheRecordsDropdownsData(string userId) { // pouze aktivní trigger
            return new HeadacheRecordsDropdownsViewModel {
                Medications = _dbContext.Medications.Where(m => m.UserId == userId && !m.IsDeleted).OrderBy(m => m.Name).ToList(),
                Treatments = _dbContext.Treatments.Where(t => t.UserId == userId && !t.IsDeleted).OrderBy(t => t.Name).ToList(),
                Triggers = _dbContext.Triggers.Where(tr => tr.UserId == userId && !tr.IsDeleted).OrderBy(tr => tr.Name).ToList()
            };
        }

        internal async Task CreateAsync(HeadacheRecordDTO newHeadacheRecord, string userId) {
            HeadacheRecord headacheRecordToInsert = await DtoToModelAsync(newHeadacheRecord, userId);
            await _dbContext.HeadacheRecords.AddAsync(headacheRecordToInsert);
            await _dbContext.SaveChangesAsync();
        }

        internal async Task<IEnumerable<HeadacheRecordDTO>> GetAll(string userId, HeadacheRecordFilterDTO filter) {
            IQueryable<HeadacheRecord> records = _dbContext.HeadacheRecords
                .Where(hr => hr.UserId == userId)
                .Include(hr => hr.Medication)
                .Include(hr => hr.Treatment)
                .Include(hr => hr.Trigger);

            // --- Logika filtrování:  ---
            if (filter.SpecificDate.HasValue) {
                DateTime startOfDay = filter.SpecificDate.Value.Date;
                DateTime endOfDay = startOfDay.AddDays(1);         // Do začátku dalšího dne (celý vybraný den)
                records = records.Where(hr => hr.Date >= startOfDay && hr.Date < endOfDay);
            }
            else {
                if (filter.DateFrom.HasValue) {
                    records = records.Where(hr => hr.Date >= filter.DateFrom.Value);
                }

                if (filter.DateTo.HasValue) {
                    records = records.Where(hr => hr.Date < filter.DateTo.Value.AddDays(1));
                }
            }
            // --- Konec logiky filtrování ---

            records = records.OrderByDescending(hr => hr.Date);

            List<HeadacheRecordDTO> headacheRecordsDtos = new List<HeadacheRecordDTO>();
            foreach (var headacheRecord in await records.ToListAsync()) {
                headacheRecordsDtos.Add(ModelToDto(headacheRecord));
            }
            return headacheRecordsDtos;
        }

        internal async Task<HeadacheRecordDTO> FindByIdAsync(int id, string userId) {
            var headacheRecordToReturn = await _dbContext.HeadacheRecords
                .Where(hr => hr.Id == id && hr.UserId == userId)
                .Include(hr => hr.Medication)
                .Include(hr => hr.Treatment)
                .Include(hr => hr.Trigger)
                .FirstOrDefaultAsync();
            if (headacheRecordToReturn == null) {
                return null;
            }
            return ModelToDto(headacheRecordToReturn);
        }
        internal async Task UpdateAsync(HeadacheRecordDTO updatedHeadacheRecord, string userId) {
            HeadacheRecord headacheRecordToSave = await DtoToModelAsync(updatedHeadacheRecord, userId);
            _dbContext.HeadacheRecords.Update(headacheRecordToSave);
            await _dbContext.SaveChangesAsync();
        }
        internal async Task DeleteAsync(int id, string userId) {
            var headacheRecordToDelete = await _dbContext.HeadacheRecords
                .FirstOrDefaultAsync(hr => hr.Id == id && hr.UserId == userId);
            if (headacheRecordToDelete != null) {
                _dbContext.HeadacheRecords.Remove(headacheRecordToDelete);
            }
            await _dbContext.SaveChangesAsync();
        }

        //METODA PRO ZJIŠTĚNÍ KOLIK DNŮ JSEM BEZ BOLESTI 
        public async Task<int> GetDaysSinceLastHeadacheRecord(string userId) {
            var lastRecord = await _dbContext.HeadacheRecords
        .Where(r => r.UserId == userId)
        .OrderByDescending(r => r.Date)
        .FirstOrDefaultAsync();

            if (lastRecord == null)
                return -1; // Nemá žádný záznam

            var days = (DateTime.Today - lastRecord.Date.Date).Days;

            return days;
        }

        //METODA PRO ZJIŠTĚNÍ Z KOLIKA PROCENT VYUŽÍVÁM LÉKY NA HEADACHE
        public async Task<(int WithMedication, int WithoutMedication, double UsagePercentage)> GetMedicationUsageSummary(string userId) {
            var records = await _dbContext.HeadacheRecords
                .Where(r => r.UserId == userId)
                .ToListAsync();

            int withMedication = records.Count(r => r.MedicationId != null);
            int withoutMedication = records.Count - withMedication;

            int total = withMedication + withoutMedication;
            double usagePercentage = total == 0 ? 0 : (withMedication * 100.0) / total;

            return (withMedication, withoutMedication, usagePercentage);
        }

        // METODA PRO ZJIŠTĚNÍ TOP TRIGGERU
        public async Task<(string TriggerName, int Count, int OtherCount)> GetTopTriggerRatioAsync(string userId) {
            var grouped = await _dbContext.HeadacheRecords
                .Where(hr => hr.UserId == userId && hr.TriggerId.HasValue)
                .GroupBy(hr => hr.TriggerId)
                .Select(g => new { TriggerId = g.Key.Value, Count = g.Count() })
                .ToListAsync();

            if (!grouped.Any())
                return ("Žádný spouštěč", 0, 0);

            var top = grouped.OrderByDescending(g => g.Count).First();
            var others = grouped.Where(g => g.TriggerId != top.TriggerId).Sum(g => g.Count);

            var triggerName = await _dbContext.Triggers
                .Where(t => t.Id == top.TriggerId)
                .Select(t => t.Name)
                .FirstOrDefaultAsync();

            return (triggerName ?? "Žádný spouštěč", top.Count, others);
        }
        public async Task<string> GetMostFrequentTriggerNameAsync(string userId) {
            var mostUsedTrigger = await _dbContext.HeadacheRecords
                .Where(r => r.UserId == userId && r.TriggerId.HasValue)
                .GroupBy(r => r.TriggerId)
                .Select(g => new {
                    TriggerId = g.Key.Value,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .FirstOrDefaultAsync();

            if (mostUsedTrigger == null)
                return "Žádný spouštěč";

            var trigger = await _dbContext.Triggers
                .FirstOrDefaultAsync(t => t.Id == mostUsedTrigger.TriggerId);

            return trigger?.Name ?? "Žádný spouštěč";
        }

        private async Task<HeadacheRecord> DtoToModelAsync(HeadacheRecordDTO newHeadacheRecord, string userId) {
            return new HeadacheRecord {
                Id = newHeadacheRecord.Id,
                Date = newHeadacheRecord.Date.Value,
                Duration = newHeadacheRecord.Duration.Value,
                Intensity = newHeadacheRecord.Intensity,
                Notes = newHeadacheRecord.Notes,
                MedicationId = newHeadacheRecord.MedicationId,
                TreatmentId = newHeadacheRecord.TreatmentId,
                TriggerId = newHeadacheRecord.TriggerId,
                UserId = userId
            };
        }
        private HeadacheRecordDTO ModelToDto(HeadacheRecord headacheRecord) {
            return new HeadacheRecordDTO {
                Id = headacheRecord.Id,
                Date = headacheRecord.Date,
                Duration = headacheRecord.Duration,
                Intensity = headacheRecord.Intensity,
                Notes = headacheRecord.Notes,
                MedicationId = headacheRecord.Medication?.Id,
                TreatmentId = headacheRecord.Treatment?.Id,
                TriggerId = headacheRecord.Trigger?.Id,
                MedicationName = headacheRecord.Medication?.Name,
                TreatmentName = headacheRecord.Treatment?.Name,
                TriggerName = headacheRecord.Trigger?.Name,
                UserId = headacheRecord.UserId

            };
        }
    }
}
