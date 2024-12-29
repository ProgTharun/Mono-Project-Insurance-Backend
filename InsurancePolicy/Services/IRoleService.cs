using InsurancePolicy.Models;

namespace InsurancePolicy.Services
{
    public interface IRoleService
    {
        public List<Role> GetAll();
        public Role GetById(Guid id);
        public Guid Add(Role role);
        public bool Update(Role role);
        public bool Delete(Guid id);
    }
}
