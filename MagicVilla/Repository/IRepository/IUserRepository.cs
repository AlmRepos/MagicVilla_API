using MagicVilla.Models;
using MagicVilla.Models.DTO;

namespace MagicVilla.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqeUser(string UserName);
        Task<LoginResponseDTO> Login(LoginRequestDTO requestDTO);
        Task<LocalUser> Register(RegisterationRequestDTO requestDTO);
    }
}
