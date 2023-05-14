using ApiMovies.Entities;
using ApiMovies.Entities.DTOs;

namespace ApiMovies.Repositories.IRepositories
{
    /// <summary>
    /// User repository interface
    /// </summary>
    public interface IUserRepository
    {
        ICollection<User> GetUsers();

        User GetUser(int userId);

        bool IsUniqueUser(string userName);

        Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto);

        Task<User> Register(UserRegisterDto userRegisterDto);
    }
}
