using GymManagmentDAL.Data.Contexts;
using GymManagmentDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repositories.Classes
{
    public class BaseEntityRepository
    {
        private readonly GymDbContext _context;

        public BaseEntityRepository(GymDbContext context)
        {
            _context = context;
        }
        public int Add(BaseEntity baseEntity)
        {
            _context.Add(baseEntity);
            return _context.SaveChanges();
        }

        public int Delete(int id)
        {
            var baseEntity = GetById(id);
            if (baseEntity is null)
                return 0;
            _context.Remove(baseEntity);
            return _context.SaveChanges();
        }

        public IEnumerable<BaseEntity> GetAll() => _context.Members.ToList();

        public BaseEntity? GetById(int id) => _context.Members.Find(id);

        public int Update(BaseEntity baseEntity)
        {
            _context.Update(baseEntity);
            return _context.SaveChanges();
        }
    }
}
