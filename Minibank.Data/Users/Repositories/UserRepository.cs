using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;

namespace Minibank.Data.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static List<UserDbModel> _userStorage = new List<UserDbModel>();

        public User GetById(string id)
        {
            var entity = _userStorage.FirstOrDefault(it => it.Id == id);

            if (entity == null)
                throw new ObjectNotFoundException($"Пользователь с Id = {id} не найден");

            return new User
            {
                Id = entity.Id,
                Login = entity.Login,
                Email = entity.Email
            };
        }

        public IEnumerable<User> GetAll()
        {
            return _userStorage.Select(it => 
            new User { 
                Id = it.Id,
                Login = it.Login,
                Email = it.Email
            });
        }

        public void Create(User user)
        {
            var entity = new UserDbModel
            {
                Id = Guid.NewGuid().ToString(),
                Login = user.Login,
                Email = user.Email
            };

            _userStorage.Add(entity);
        }

        public void Delete(string id)
        {
            var entity = _userStorage.FirstOrDefault(it => it.Id == id);

            if (entity == null)
                throw new ObjectNotFoundException($"Пользователь с Id = {id} не найден");

            _userStorage.Remove(entity);
        }

        public void Update(User user)
        {
            var entity = _userStorage.FirstOrDefault(it => it.Id == user.Id);

            if (entity == null)
                throw new ObjectNotFoundException($"Пользователь с Id = {user.Id} не найден");

            entity.Login = user.Login;
            entity.Email = user.Email;    
        }
    }
}
