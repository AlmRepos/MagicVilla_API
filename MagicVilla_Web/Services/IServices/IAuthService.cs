using MagicVilla.Models;
using MagicVilla_Web.Models.DTO;

namespace MagicVilla_Web.Services.IServices
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDTO requestDTO);
        Task<T> RegisterAsync<T>(RegisterationRequestDTO requestDTO);
    }
}
