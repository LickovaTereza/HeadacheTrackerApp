namespace HeadacheTracker.Models {
    public class Treatment {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? UserId { get; set; }
        public AppUser? User { get; set; }

        public bool IsDeleted { get; set; } = false;

    }
}
