using AutoMapper;
using InsurancePolicy.Exceptions.RoleException;
using InsurancePolicy.Models;
using InsurancePolicy.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Stripe;

namespace InsurancePolicy.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRepository<Role> _repository;
        public RoleService(IRepository<Role> repository)
        {
            _repository = repository;
        }
        public Guid Add(Role role)
        {
            _repository.Add(role);
            Log.Information("New record added:" + role.Id);
            return role.Id;
        }

        public bool Delete(Guid id)
        {
            var Role = _repository.GetById(id);
            if (Role == null)
            {
                throw new RoleNotFoundException("No such role found to delete");
            }
            _repository.Delete(Role);
            Log.Information("Record upadted", id);// Log the count of records retrieved

            return true;
            
        }

        public Role GetById(Guid id)
        {
            var role = _repository.GetById(id);
            if (role != null)
            {
                Log.Information("Record upadted", id);// Log the count of records retrieved

                return role;
            }
            throw new RoleNotFoundException("No such role found");
        }

        public List<Role> GetAll()
        {
            var roles = _repository.GetAll().Include(a=>a.Users).ToList();
            if (roles.Count != 0)
            {
                Log.Information("Records Retrieved. Count: {count}", roles.Count);// Log the count of records retrieved

                return roles;
            }
            throw new RolesDoesNotExistException("No roles Exist");
        }

        public bool Update(Role role)
        {
            var existingRole = _repository.GetAll().AsNoTracking().FirstOrDefault(a => a.Id == role.Id);
            if (existingRole != null)
            {
                _repository.Update(role);
                Log.Information("Record upadted", existingRole.Id);// Log the count of records retrieved

                return true;
            }
            throw new RoleNotFoundException("No such role found");
        }
    }
}
