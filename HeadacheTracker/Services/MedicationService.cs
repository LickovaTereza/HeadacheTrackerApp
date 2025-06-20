using HeadacheTracker.DTO;
using HeadacheTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace HeadacheTracker.Services {
    public class MedicationService {
        private readonly AppDbContext _dbContext;
        public MedicationService(AppDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<MedicationDTO>> GetAll(string userId) {
            // 1. Načti všechny medikamenty pro daného uživatele
            var userMedications = await _dbContext.Medications
                                                .Where(m => m.UserId == userId && !m.IsDeleted)
                                                .OrderBy(m => m.Name)
                                                .ToListAsync();

            // 2. Načti VŠECHNY záznamy o bolestech hlavy pro daného uživatele
            var allHeadacheRecords = await _dbContext.HeadacheRecords
                                                    .Where(hr => hr.UserId == userId)
                                                    .ToListAsync();

            var medicationDtos = new List<MedicationDTO>();

            // 3. Pro každý medikament spočítáme jeho použití
            foreach (var medication in userMedications) {
                var usageCount = allHeadacheRecords.Count(hr => hr.MedicationId == medication.Id);
                medicationDtos.Add(ModelToDto(medication, usageCount));
            }


            // 5. Přidáme "Žádný medikament" jako nový DTO objekt, pokud existují takové záznamy
            var noMedicationCount = allHeadacheRecords.Count(hr => !hr.MedicationId.HasValue || hr.MedicationId == null);

            if (noMedicationCount > 0) {
                medicationDtos.Add(new MedicationDTO {
                    Id = -1, // Speciální ID pro "Žádný medikament"
                    Name = "Žádný medikament", // Název, který se zobrazí v grafu
                    UsageCount = noMedicationCount,
                    UserId = userId // Uživatel, ke kterému se to vztahuje
                });
            }

            return medicationDtos.OrderBy(m => m.Name); // když dám m => m.UsageCount tak se seřadí podle četnosti (ale i výpis, nejen graf)
        }

        public async Task CreateAsync(MedicationDTO newMedication, string userId) {
            Medication medicationToSave = DtoToModel(newMedication, userId);
            await _dbContext.Medications.AddAsync(medicationToSave);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<MedicationDTO> GetByIdAsync(int id, string userId) {
            var medicationToEdit = await _dbContext.Medications
                                                    .Where(m => m.Id == id && m.UserId == userId)
                                                    .FirstOrDefaultAsync();

            if (medicationToEdit == null) {
                return null;
            }

            var usageCount = await _dbContext.HeadacheRecords.CountAsync(hr => hr.MedicationId == medicationToEdit.Id && hr.UserId == userId);
            return ModelToDto(medicationToEdit, usageCount);
        }

        internal async Task UpdateAsync(MedicationDTO medicationDTO, int id, string userId) {
            var existingMedication = await _dbContext.Medications
                                                    .Where(m => m.Id == id && m.UserId == userId)
                                                    .FirstOrDefaultAsync();

            if (existingMedication == null) {
                return;
            }

            existingMedication.Name = medicationDTO.Name;
            _dbContext.Medications.Update(existingMedication);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> SoftDeleteAsync(int id, string userId) {
            var medicationToDelete = await _dbContext.Medications
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (medicationToDelete == null)
                return false;

            // Zkontroluj, zda je trigger použit v HeadacheRecords
            bool isUsed = await _dbContext.HeadacheRecords
                .AnyAsync(hr => hr.MedicationId == id && hr.UserId == userId);

            if (isUsed) {
                // Nelze smazat, protože je využíván
                return false;
            }

            // Pokud není použit, proveď soft delete
            medicationToDelete.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        // transformační metoda Model -> DTO
        private MedicationDTO ModelToDto(Medication medication, int usageCount = 0) {
            return new MedicationDTO {
                Id = medication.Id,
                Name = medication.Name,
                UserId = medication.UserId,
                UsageCount = usageCount

            };
        }
        // transformační metoda DTO -> Model
        private Medication DtoToModel(MedicationDTO newMedication, string userId) {
            return new Medication {
                Id = newMedication.Id,
                Name = newMedication.Name,
                UserId = userId
            };
        }
    }
}