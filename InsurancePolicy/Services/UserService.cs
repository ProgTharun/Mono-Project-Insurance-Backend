using InsurancePolicy.Exceptions.RoleException;
using InsurancePolicy.Exceptions.UserExceptions;
using InsurancePolicy.Models;
using InsurancePolicy.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace InsurancePolicy.Services
{
    public class UserService:IUserService
    {
        private readonly IRepository<User> _repository;
        public UserService(IRepository<User> repository)
        {
            _repository = repository;
        }
        public Guid Add(User user)
        {
            _repository.Add(user);
            Log.Information("New record added:" + user.UserId);
            return user.UserId;
        }

        public bool Delete(Guid id)
        {
            var user = _repository.GetById(id);
            if (user != null)
            {
                _repository.Delete(user);
                Log.Information("Record upadted", id);// Log the count of records retrieved

                return true;
            }
            throw new UserNotFoundException("No such User found to delete");
        }

        public User GetById(Guid id)
        {
            var user = _repository.GetById(id);
            if (user != null)
            {
                Log.Information("Record upadted", id);// Log the count of records retrieved

                return user;
            }
            throw new UserNotFoundException("No such User found");
        }

        public List<User> GetAll()
        {
            var users = _repository.GetAll().Include(r=>r.Role).ToList();
            if (users.Count == 0)
                throw new UsersDoesNotExistException("No Users Exist");
            Log.Information("Records Retrieved. Count{count}", users.Count);

            return users;
            
        }

        public bool Update(User User)
        {
            var existingUser = _repository.GetAll().AsNoTracking().FirstOrDefault(u=>u.UserId == User.UserId);
            if (existingUser != null)
            {
                _repository.Update(User);
                Log.Information("Record upadted", existingUser.UserId);// Log the count of records retrieved

                return true;
            }
            throw new UserNotFoundException("No such User found");
        }
    }
}
