using MagicVilla_Web.Models;

namespace MagicVilla_Web.Services.IServices
{
    public interface IBaseService
    {
        APIResponse response { get; set; }

        Task<T> SendAsyc<T>(APIRequest apiRequest);
    }
}
