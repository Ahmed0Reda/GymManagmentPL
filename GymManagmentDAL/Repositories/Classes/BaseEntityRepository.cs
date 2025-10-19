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
    public class BaseEntityRepository : GenericRepository<BaseEntity>, IBaseEntityRepository
    {
        public BaseEntityRepository(GymDbContext context) : base(context)
        {
        }
    }
}
