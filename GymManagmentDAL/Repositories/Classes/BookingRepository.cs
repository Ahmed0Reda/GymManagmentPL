using GymManagmentDAL.Data.Contexts;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repositories.Classes
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(GymDbContext context) : base(context)
        {
        }
    }
}
