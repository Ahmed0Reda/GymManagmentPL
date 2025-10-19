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
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        public MemberRepository(GymDbContext context) : base(context)
        {
        }
    }
}
