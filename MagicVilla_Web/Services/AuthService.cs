using MagicVilla.Models;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class AuthService :BaseService, IAuthService
    {
        private readonly IHttpClientFactory clientFactory;
        string VillaUrl;
        public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            this.clientFactory = clientFactory;
            VillaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }



        public Task<T> LoginAsync<T>(LoginRequestDTO requestDTO)
        {
            return SendAsyc<T>(new APIRequest()
            {
                APIType = SD.APIType.POST,
                Data = requestDTO,
                URL = VillaUrl + "/api/Users/login"
            });
        }

        public Task<T> RegisterAsync<T>(RegisterationRequestDTO requestDTO)
        {
            return SendAsyc<T>(new APIRequest()
            {
                APIType = SD.APIType.POST,
                Data = requestDTO,
                URL = VillaUrl + "/api/Users/register"
            });
        }
    }
}
