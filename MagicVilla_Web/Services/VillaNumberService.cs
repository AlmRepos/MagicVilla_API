using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class VillaNumberService : BaseService, IVillaNumberService
    {
        private readonly IHttpClientFactory clientFactory;
        string VillaUrl;
        public VillaNumberService(IHttpClientFactory clientFactory,IConfiguration configuration): base(clientFactory)
        {
            this.clientFactory = clientFactory;
            VillaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public Task<T> CreateAsync<T>(VillaNumberCreateDTO dto, string token)
        {
            return SendAsyc<T>(new APIRequest()
            {
                APIType = SD.APIType.POST,
                Data = dto,
                URL = VillaUrl + "/api/VillaNumber",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsyc<T>(new APIRequest()
            {
                APIType = SD.APIType.DELETE,
                URL = VillaUrl + "/api/VillaNumber/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsyc<T>(new APIRequest()
            {
                APIType = SD.APIType.GET,

                URL = VillaUrl + "/api/VillaNumber",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsyc<T>(new APIRequest()
            {
                APIType = SD.APIType.GET,
                URL = VillaUrl + "/api/VillaNumber/" + id,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto, string token)
        {
            return SendAsyc<T>(new APIRequest()
            {
                APIType = SD.APIType.PUT,
                URL=VillaUrl + "/api/VillaNumber/"+dto.VillaNo,
                Data = dto,
                Token = token
            });
        }
    }
}
