using Microsoft.EntityFrameworkCore;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Minibank.Data.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> GetById(string id, CancellationToken cancellationToken)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(it => it.Id == id, cancellationToken: cancellationToken);

            if (entity == null)
                throw new ObjectNotFoundException($"Пользователь с Id = {id} не найден");

            return new User
            {
                Id = entity.Id,
                Login = entity.Login,
                Email = entity.Email
            };
        }

        public async Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.Users.Select(it => 
            new User { 
                Id = it.Id,
                Login = it.Login,
                Email = it.Email
            }).ToListAsync(cancellationToken);
        }

        public async Task Create(User user, CancellationToken cancellationToken)
        {
            var entity = new UserDbModel
            {
                Id = Guid.NewGuid().ToString(),
                Login = user.Login,
                Email = user.Email
            };

            await _context.Users.AddAsync(entity, cancellationToken);
        }

        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(it => it.Id == id, cancellationToken: cancellationToken);

            if (entity == null)
                throw new ObjectNotFoundException($"Пользователь с Id = {id} не найден");

            _context.Users.Remove(entity);
        }

        public async Task Update(User user, CancellationToken cancellationToken)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(it => it.Id == user.Id, cancellationToken);

            if (entity == null)
                throw new ObjectNotFoundException($"Пользователь с Id = {user.Id} не найден");

            entity.Login = user.Login;
            entity.Email = user.Email;    
        }
    }
}
