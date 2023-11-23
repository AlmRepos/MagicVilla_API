using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class VillaService : BaseService, IVillaService
    {
        private readonly IHttpClientFactory httpClient;
        string VillaUrl;


        public VillaService(IHttpClientFactory httpClient,IConfiguration configuration):base(httpClient)
        {
            this.httpClient = httpClient;
            VillaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }


        public Task<T> CreateAsync<T>(VillaCreateDTO dto, string token)
        {
            return SendAsyc<T>(new APIRequest()
            {
                APIType = SD.APIType.POST,
                Data = dto,
                URL = VillaUrl + "/api/VillaAPI",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsyc<T>(new APIRequest()
            {
                APIType = SD.APIType.DELETE,
                URL = VillaUrl + "/api/VillaAPI/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsyc<T>(new APIRequest()
            {
                APIType = SD.APIType.GET,
                URL = VillaUrl + "/api/VillaAPI",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsyc<T>(new APIRequest()
            {
                APIType = SD.APIType.GET,
                URL = VillaUrl + "/api/VillaAPI/" + id,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(VillaUpdateDTO dto, string token)
        {
            return SendAsyc<T>(new APIRequest()
            {
                APIType = SD.APIType.PUT,
                Data = dto,
                URL = VillaUrl + "/api/VillaAPI/"+dto.Id,
                Token = token
            });
        }
    }
}
