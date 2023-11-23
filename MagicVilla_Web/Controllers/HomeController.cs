using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MagicVilla_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IVillaService villaNumber;
        private readonly IMapper mapper;

        public HomeController(IVillaService villaNumber, IMapper mapper)
        {
            this.villaNumber = villaNumber;
            this.mapper = mapper;
        }


        public async Task<IActionResult> Index()
        {
            List<VillaDTO> list = new();
            var response = await villaNumber.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(response.Result.ToString());
            }
            return View(list);
        }
    }
}