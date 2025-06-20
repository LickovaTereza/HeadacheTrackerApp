namespace HeadacheTracker.ViewModels {
    public class HomePageViewModel {
        public int DaysWithoutHeadache { get; set; }
        public int MedicationWithUsage { get; set; }
        public int MedicationWithoutUsage { get; set; }
        public double UsagePercentage { get; set; } 
        public string MostFrequentTrigger { get; set; }
        public string TopTriggerName { get; set; }
        public int TopTriggerCount { get; set; }
        public int OtherTriggersCount { get; set; }
    }
}
