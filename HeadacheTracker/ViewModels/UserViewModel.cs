using System.ComponentModel.DataAnnotations;

namespace HeadacheTracker.ViewModels {
    public class UserViewModel {
        [Required(ErrorMessage = "Údaj je povinný")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Údaj je povinný")]
        [EmailAddress(ErrorMessage ="Zadejte platnou e-mailovou adresu")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Zadejte heslo.")]
        [MinLength(8, ErrorMessage = "Heslo musí mít alespoň 8 znaků.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Heslo musí obsahovat alespoň jedno velké písmeno a číslo.")]
        public string Password { get; set; }
    }
}
