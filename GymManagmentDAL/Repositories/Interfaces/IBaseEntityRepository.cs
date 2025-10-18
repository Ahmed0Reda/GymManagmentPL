using GymManagmentDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repositories.Interfaces
{
    public interface IBaseEntityRepository
    {
        BaseEntity? GetById(int id);
        IEnumerable<BaseEntity> GetAll();
        int Add(BaseEntity baseEntity);
        int Update(BaseEntity baseEntity);
        int Delete(int id);
    }
}
