namespace HeadacheTracker.Models {
    public class HeadacheRecord {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; } 
        public int Intensity { get; set; } 

        public int? MedicationId { get; set; }
        public int? TreatmentId { get; set; }
        public int? TriggerId { get; set; }

        public Medication? Medication { get; set; }
        public Treatment? Treatment { get; set; }
        public Trigger? Trigger { get; set; }
        public string? Notes { get; set; }

        public string UserId { get; set; }

    }
}
