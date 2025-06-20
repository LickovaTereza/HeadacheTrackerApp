using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace HeadacheTracker.Models {
    public class AppUser : IdentityUser {
        [Required]
        [EmailAddress(ErrorMessage = "Zadejte platný e-mail.")]
        public override string Email { get; set; }
        public AppUser() {
            Medications = new HashSet<Medication>();
            Treatments = new HashSet<Treatment>();
            Triggers = new HashSet<Trigger>();
        }

        public ICollection<Medication> Medications { get; set; }
        public ICollection<Treatment> Treatments { get; set; }
        public ICollection<Trigger> Triggers { get; set; }
    }
}
