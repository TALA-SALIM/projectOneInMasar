using System.ComponentModel.DataAnnotations;
using homework.Enums;

namespace homework.ViewModel
{
    public class CreatUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Range(1, 2, ErrorMessage = "Choose Admin or Employee.")]
        [Display(Name = "Role")]
        public UserType UserType { get; set; } = homework.Enums.UserType.Employee;
    }
}