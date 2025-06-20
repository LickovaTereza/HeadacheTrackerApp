using System.ComponentModel.DataAnnotations;
using HeadacheTracker.Models;

namespace HeadacheTracker.DTO {
    public class MedicationDTO {
        public int Id { get; set; }

        [Required(ErrorMessage = "Povinný údaj")]
        [MaxLength(30)]
        public string Name { get; set; }
        public string? UserId { get; set; }
        public int UsageCount { get; set; }

        public bool IsDeleted { get; set; }



    }
}
