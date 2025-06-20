using HeadacheTracker.DTO;

namespace HeadacheTracker.ViewModels {
    public class MedicationViewModel {
        public MedicationDTO Medication { get; set; }
        public IEnumerable<MedicationDTO> MedicationsForChart { get; set; }
    }
}
