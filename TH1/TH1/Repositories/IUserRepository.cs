using TH1.Models;

namespace TH1.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
    }
}
