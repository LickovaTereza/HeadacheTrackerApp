using HeadacheTracker.DTO;
using HeadacheTracker.Models;

namespace HeadacheTracker.ViewModels {
    public class HeadacheRecordsViewModel { // HeadacheRecordsViewModel přidaný pro filtraci
        public IEnumerable<HeadacheRecordDTO> Records { get; set; } 
        public HeadacheRecordFilterDTO Filter { get; set; }
        public HeadacheRecordsDropdownsViewModel Dropdowns { get; set; }
    }
    public class HeadacheRecordsDropdownsViewModel {
        public IEnumerable<Treatment> Treatments { get; set; }
        public IEnumerable<Trigger> Triggers { get; set; }
        public IEnumerable<Medication> Medications { get; set; }
        public HeadacheRecordsDropdownsViewModel() {
            Treatments = new List<Treatment>();
            Triggers = new List<Trigger>();
            Medications = new List<Medication>();
        }
    }
}

