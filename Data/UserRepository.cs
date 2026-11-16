using DotnetAPI.Models;

namespace DotnetAPI.Data
{
    public class UserRepository(IConfiguration config) : IUserRepository
    {
        readonly DataContextEF _entityFramework = new(config);

        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }

        public void AddEntity<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _entityFramework.Add(entityToAdd);
            }
        }

        public void RemoveEntity<T>(T entityToRemove)
        {
            if (entityToRemove != null)
            {
                _entityFramework.Remove(entityToRemove);
            }
        }

        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = [.. _entityFramework.Users];
            return users;
        }

        public User GetSingleUser(int userId)
        {
            User? user = _entityFramework.Users
                .Where(u => u.UserId == userId)
                .FirstOrDefault();

            if (user != null)
            {
                return user;
            }

            throw new Exception("User not found");
        }

        public UserSalary GetSingleUserSalary(int userId)
        {
            UserSalary? userSalary = _entityFramework.UserSalary
                .Where(u => u.UserId == userId)
                .FirstOrDefault();

            if (userSalary != null)
            {
                return userSalary;
            }

            throw new Exception("User not found");
        }

        public UserJobInfo GetSingleUserJobInfo(int userId)
        {
            UserJobInfo? userJobInfo = _entityFramework.UserJobInfo
                .Where(u => u.UserId == userId)
                .FirstOrDefault();

            if (userJobInfo != null)
            {
                return userJobInfo;
            }

            throw new Exception("User not found");
        }
    }
}