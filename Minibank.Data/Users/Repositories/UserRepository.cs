﻿using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Minibank.Data.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static List<UserDbModel> userStorage = new List<UserDbModel>();

        public User Get(string id)
        {
            var entity =  userStorage.FirstOrDefault(it => it.Id == id);

            if (entity == null)
                return null;

            return new User
            {
                Id = entity.Id,
                Login = entity.Login,
                Email = entity.Email
            };
        }

        public IEnumerable<User> GetAll()
        {
            return userStorage.Select(it => 
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

            userStorage.Add(entity);
        }

        public void Delete(string id)
        {
            var entity = userStorage.FirstOrDefault(it => it.Id == id);

            if (entity != null)
                userStorage.Remove(entity);
        }

        public void Update(User user)
        {
            var entity = userStorage.FirstOrDefault(it => it.Id == user.Id);

            if (entity != null)
            {
                entity.Login = user.Login;
                entity.Email = user.Email;
            }
                
        }
    }
}
