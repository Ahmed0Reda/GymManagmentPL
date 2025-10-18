using GymManagmentDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repositories.Interfaces
{
    public interface IMembershipRepository
    {
        Membership? GetById(int id);
        IEnumerable<Membership> GetAll();
        int Add(Membership membership);
        int Update(Membership membership);
        int Delete(int id);
    }
}
