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
    public class GymUserRepository : IGymUserRepository
    {
        private readonly GymDbContext _context;

        public GymUserRepository(GymDbContext context)
        {
            _context = context;
        }
        public int Add(GymUser gymUser)
        {
            _context.Add(gymUser);
            return _context.SaveChanges();
        }

        public int Delete(int id)
        {
            var gymUser = GetById(id);
            if (gymUser is null)
                return 0;
            _context.Remove(gymUser);
            return _context.SaveChanges();
        }

        public IEnumerable<GymUser> GetAll() => _context.Members.ToList();

        public GymUser? GetById(int id) => _context.Members.Find(id);

        public int Update(GymUser gymUser)
        {
            _context.Update(gymUser);
            return _context.SaveChanges();
        }
    }
}
