using System.Linq;
using HeadacheTracker.DTO;
using HeadacheTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace HeadacheTracker.Services {
    public class TriggerService {
        private AppDbContext _dbContext;
        public TriggerService(AppDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TriggerDTO>> GetAll(string userId) {
            // 1. Načti triggery pro daného uživatele
            var allTriggers = await _dbContext.Triggers
                                                .Where(t => t.UserId == userId && !t.IsDeleted)
                                                .OrderBy(t => t.Name)
                                                .ToListAsync();

            // 2. Načti všechny záznamy o bolestech hlavy
            var allHeadacheRecords = await _dbContext.HeadacheRecords
                                                     .Where(hr => hr.UserId == userId)
                                                     .ToListAsync();

            var triggerDtos = new List<TriggerDTO>();

            // 3. Pro každý trigger spočítáme jeho použití
            foreach (var trigger in allTriggers) {
                var usageCount = allHeadacheRecords.Count(hr => hr.TriggerId == trigger.Id);
                triggerDtos.Add(ModelToDto(trigger, usageCount));
            }

            // 5. Záznamy bez triggeru
            var noTriggerCount = allHeadacheRecords.Count(hr => !hr.TriggerId.HasValue || hr.TriggerId == null);

            if (noTriggerCount > 0) {
                triggerDtos.Add(new TriggerDTO {
                    Id = -1,
                    Name = "Žádný spouštěč",
                    UsageCount = noTriggerCount,
                    UserId = userId
                });
            }

            return triggerDtos.OrderBy(t => t.Name);
        }


        public async Task CreateAsync(TriggerDTO newTrigger, string userId) {
            Trigger triggerToSave = DtoToModel(newTrigger, userId);
            await _dbContext.Triggers.AddAsync(triggerToSave);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<TriggerDTO> GetByIdAsync(int id, string userId) {
            var triggerToEdit = await _dbContext.Triggers
                                                    .Where(m => m.Id == id && m.UserId == userId)
                                                    .FirstOrDefaultAsync();
            if (triggerToEdit == null) {
                return null;
            }

            var usageCount = await _dbContext.HeadacheRecords.CountAsync(hr => hr.TriggerId == triggerToEdit.Id && hr.UserId == userId);
            return ModelToDto(triggerToEdit, usageCount);

        }

        internal async Task UpdateAsync(TriggerDTO triggerDTO, int id, string userId) {
            var existingTrigger = await _dbContext.Triggers
                                                    .Where(t => t.Id == id && t.UserId == userId)
                                                    .FirstOrDefaultAsync();

            if (existingTrigger == null) {
                return;
            }

            existingTrigger.Name = triggerDTO.Name;
            _dbContext.Triggers.Update(existingTrigger);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> SoftDeleteAsync(int id, string userId) {
            var triggerToDelete = await _dbContext.Triggers
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (triggerToDelete == null)
                return false; 

            // Zkontroluj, zda je trigger použit v HeadacheRecords
            bool isUsed = await _dbContext.HeadacheRecords
                .AnyAsync(hr => hr.TriggerId == id && hr.UserId == userId);

            if (isUsed) {
                // Nelze smazat, protože je využíván
                return false;
            }

            // Pokud není použit, tak soft delete
            triggerToDelete.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        // transformační metoda Model -> DTO
        private TriggerDTO ModelToDto(Trigger trigger, int usageCount = 0) {
            return new TriggerDTO {
                Id = trigger.Id,
                Name = trigger.Name,
                UserId = trigger.UserId,
                UsageCount = usageCount,
            };
        }
        // transformační metoda DTO -> Model
        private Trigger DtoToModel(TriggerDTO newTrigger, string userId) {
            return new Trigger {
                Id = newTrigger.Id,
                Name = newTrigger.Name,
                UserId = userId,

            };
        }
    }
}
