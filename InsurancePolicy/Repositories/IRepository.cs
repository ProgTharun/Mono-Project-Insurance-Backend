namespace InsurancePolicy.Repositories
{
    public interface IRepository<T>
    {
        public void Add(T entity);
        public T Update(T entity);
        public void Delete(T entity);
        public void Activate(T entity);
        public T GetById(Guid id);
        public IQueryable<T> GetAll();
    }
}
