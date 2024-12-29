using InsurancePolicy.Models;

namespace InsurancePolicy.Services
{
    public interface IUserService
    {
        public List<User> GetAll();
        public User GetById(Guid id);
        public Guid Add(User user);
        public bool Update(User user);
        public bool Delete(Guid id);
    }
}
