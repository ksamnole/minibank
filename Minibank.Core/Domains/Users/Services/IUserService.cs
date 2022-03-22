using System.Collections.Generic;

namespace Minibank.Core.Domains.Users.Services
{
    public interface IUserService
    {
        User GetById(string id);
        IEnumerable<User> GetAll();
        void Create(User user);
        void Update(User user);
        void Delete(string id);
    }
}
