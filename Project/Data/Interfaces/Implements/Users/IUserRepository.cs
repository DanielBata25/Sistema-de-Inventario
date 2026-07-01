using Data.Interfaces.IRepository;
using Entity.Model.Auth;

namespace Data.Interfaces.Implements.Users
{
    public interface IUserRepository : IDataGeneric<User>
    {
        Task<User?> GetByEmailAsync(string email);

        Task<bool> EmailExistsAsync(string email);
    }
}