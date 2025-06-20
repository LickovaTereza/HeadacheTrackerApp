using HeadacheTracker.DTO;
using HeadacheTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace HeadacheTracker.Services {
    public class TreatmentService {
        private readonly AppDbContext _dbContext;
        public TreatmentService(AppDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TreatmentDTO>> GetAll(string userId) { 
            // 1. Načti všechny treatmenty pro daného uživatele
            var userTreatments = await _dbContext.Treatments
                                                .Where(t => t.UserId == userId && !t.IsDeleted)
                                                .OrderBy(t => t.Name)
                                                .ToListAsync();

            // 2. Načti VŠECHNY záznamy o bolestech hlavy pro daného uživatele
            var allHeadacheRecords = await _dbContext.HeadacheRecords
                                                    .Where(hr => hr.UserId == userId)
                                                    .ToListAsync();

            var treaetmentDtos = new List<TreatmentDTO>();

            // 3. Pro každý treatment spočítáme jeho použití
            foreach (var treatment in userTreatments) {
                var usageCount = allHeadacheRecords.Count(hr => hr.TreatmentId == treatment.Id);
                treaetmentDtos.Add(ModelToDto(treatment, usageCount));
            }


            // 5. Přidáme "Žádný treatment" jako nový DTO objekt, pokud existují takové záznamy
            var noTreatmentCount = allHeadacheRecords.Count(hr => !hr.TreatmentId.HasValue || hr.TreatmentId == null);

            if (noTreatmentCount > 0) {
                treaetmentDtos.Add(new TreatmentDTO {
                    Id = -1, // Speciální ID pro "Žádný treatment"
                    Name = "Žádná další léčba", // Název, který se zobrazí v grafu
                    UsageCount = noTreatmentCount,
                    UserId = userId // Uživatel, ke kterému se to vztahuje
                });
            }

            // Vrátíme všechny DTO objekty (včetně "Žádný treatment"), seřazené podle názvu
            return treaetmentDtos.OrderBy(t => t.Name);
        }

        public async Task CreateAsync(TreatmentDTO newTreatment, string userId) {
            Treatment treatmentToSave = DtoToModel(newTreatment, userId);
            await _dbContext.Treatments.AddAsync(treatmentToSave);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<TreatmentDTO> GetByIdAsync(int id, string userId) {
            var treatmentToEdit = await _dbContext.Treatments
                                                    .Where(m => m.Id == id && m.UserId == userId)
                                                    .FirstOrDefaultAsync();
            if (treatmentToEdit == null) {
                return null;
            }

            var usageCount = await _dbContext.HeadacheRecords.CountAsync(hr => hr.TreatmentId == treatmentToEdit.Id && hr.UserId == userId);
            return ModelToDto(treatmentToEdit, usageCount);

        }

        internal async Task UpdateAsync(TreatmentDTO treatmentDTO, int id, string userId) {
            var existingTreatment = await _dbContext.Treatments
                                                    .Where(t => t.Id == id && t.UserId == userId)
                                                    .FirstOrDefaultAsync();

            if (existingTreatment == null) {
                return;
            }

            existingTreatment.Name = treatmentDTO.Name;
            _dbContext.Treatments.Update(existingTreatment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> SoftDeleteAsync(int id, string userId) {
            var treatmentToDelete = await _dbContext.Treatments
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (treatmentToDelete == null)
                return false; 

            // Zkontroluj, zda je trigger použit v HeadacheRecords
            bool isUsed = await _dbContext.HeadacheRecords
                .AnyAsync(hr => hr.TreatmentId == id && hr.UserId == userId);

            if (isUsed) {
                // Nelze smazat, protože je využíván
                return false;
            }

            // Pokud není použit, proveď soft delete
            treatmentToDelete.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        // transformační metoda Model -> DTO
        private TreatmentDTO ModelToDto(Treatment treatment, int usageCount = 0) {
            return new TreatmentDTO {
                Id = treatment.Id,
                Name = treatment.Name,
                UserId = treatment.UserId,
                UsageCount = usageCount
            };
        }
        // transformační metoda DTO -> Model
        private Treatment DtoToModel(TreatmentDTO newTreatment, string userId) {
            return new Treatment {
                Id = newTreatment.Id,
                Name = newTreatment.Name,
                UserId = userId
            };
        }
    }
}
