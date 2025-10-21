using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymmanagmentBLL.ViewModels
{
    public class HealthRecordViewModel
    {
        [Range (0.1, 300, ErrorMessage = "Height must be greater Than 0")]
        public decimal Height { get; set; }
        [Range(0.1, 500, ErrorMessage = "Weight must be greater Than 0")]
        public decimal Weight { get; set; }
        [Required(ErrorMessage = "Blood Type is required")]
        [StringLength(3, ErrorMessage = "Blood Type can't be longer than 3 characters")]
        public string BloodType { get; set; } = null!;
        public string? Note { get; set; }
    }
}
