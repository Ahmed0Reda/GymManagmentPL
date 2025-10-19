using GymManagmentDAL.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymmanagmentBLL.ViewModels.MemberViewModels
{
    public class MemberToUpdateViewModel
    {
        public string Name { get; set; } = null!;
        public string? Photo { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [RegularExpression(@"^(010|011|012|015)\d{8}", ErrorMessage = "Phone number must be in Egytion format")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;
        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public int BuildingNumber { get; set; }
        [Required(ErrorMessage = "City is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "City must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City can only contain letters and spaces")]
        public string City { get; set; } = null!;
        [Required(ErrorMessage = "Street is required")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Street must be between 2 and 150 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Street can only contain letters, numbers, and spaces")]
        public string Street { get; set; } = null!;
    }
}
