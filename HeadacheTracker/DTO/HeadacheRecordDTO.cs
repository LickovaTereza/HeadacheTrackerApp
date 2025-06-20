using System.ComponentModel.DataAnnotations;
using HeadacheTracker.Models;


namespace HeadacheTracker.DTO {
    public class HeadacheRecordDTO {
        public int Id { get; set; }
        [Required(ErrorMessage = "Povinný údaj")]
        public DateTime? Date { get; set; }
        [Required(ErrorMessage = "Povinný údaj")]

        public int? Duration { get; set; } 
        public int Intensity { get; set; } 
        public int? MedicationId { get; set; }
        public int? TreatmentId { get; set; }
        public int? TriggerId { get; set; }
        public string? MedicationName { get; set; }
        public string? TreatmentName { get; set; }
        public string? TriggerName { get; set; }
        public string? Notes { get; set; }

        public string? UserId { get; set; }

    }
}
