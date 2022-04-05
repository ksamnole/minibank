using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Users.Services
{
    public interface IUserService
    {
        Task<User> GetById(string id);
        Task<IEnumerable<User>> GetAll();
        Task Create(User user);
        Task Update(User user);
        Task Delete(string id);
    }
}
