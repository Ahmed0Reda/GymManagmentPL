using GymManagmentDAL.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymmanagmentBLL.ViewModels.TrainerViewModels
{
    public class SpecialitiesViewModel
    {
        public Specialities selectedSpecialities { get; set; }
        public List<Specialities> SpecialitiesList{ get; set; }
    }
}
