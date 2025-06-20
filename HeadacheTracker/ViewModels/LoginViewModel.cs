using System.ComponentModel.DataAnnotations;

namespace HeadacheTracker.ViewModels {
    public class LoginViewModel {
        [Required(ErrorMessage = "Vyplňte uživatelské jméno")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Vyplňte heslo")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public bool Remember { get; set; }
    }
}
