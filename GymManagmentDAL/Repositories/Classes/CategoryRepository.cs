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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly GymDbContext _context;

        public CategoryRepository(GymDbContext context)
        {
            _context = context;
        }
        public int Add(Category category)
        {
            _context.Add(category);
            return _context.SaveChanges();
        }

        public int Delete(int id)
        {
            var category = GetById(id);
            if (category is null)
                return 0;
            _context.Remove(category);
            return _context.SaveChanges();
        }

        public IEnumerable<Category> GetAll() => _context.Categories.ToList();

        public Category? GetById(int id) => _context.Categories.Find(id);

        public int Update(Category category)
        {
            _context.Update(category);
            return _context.SaveChanges();
        }
    }
}
