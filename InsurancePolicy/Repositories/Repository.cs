using InsurancePolicy.Data;
using Microsoft.EntityFrameworkCore;

namespace InsurancePolicy.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _table;
        public Repository(AppDbContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _table.Add(entity);
            _context.SaveChanges();

        }

        public void Delete(T entity)
        {
            // Assuming "status" is a property in the entity that represents the soft delete state
            var propertyInfo = entity.GetType().GetProperty("Status");
            if (propertyInfo != null && propertyInfo.PropertyType == typeof(bool))
            {
                propertyInfo.SetValue(entity, false);
                _table.Update(entity); // Mark the entity as updated
                _context.SaveChanges(); // Persist changes to the database
            }
            else
            {
                throw new InvalidOperationException("The entity does not have a 'status' property of type bool.");
            }
        }
        public void Activate(T entity)
        {
            var propertyInfo = entity.GetType().GetProperty("Status");
            if (propertyInfo != null && propertyInfo.PropertyType == typeof(bool))
            {
                propertyInfo.SetValue(entity, true);
                _table.Update(entity);
                _context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("The entity does not have a 'status' property of type bool.");
            }
        }

        public IQueryable<T> GetAll()
        {
            return _table.AsQueryable();
        }

        public T GetById(Guid id)
        {
            var entity = _table.Find(id);
            return entity;
        }

        public T Update(T entity)
        {
            _table.Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
