using Business.Interfaces.IBusiness;
using Entity.DTOs.Users;

namespace Business.Interfaces.Implements.Users
{
    public interface IUserService : IBusiness<UserCreateDto, UserUpdateDto, UserSelectDto>
    {
        Task<UserSelectDto?> GetByEmailAsync(string email);
    }
}