using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService villaService;
        private readonly IMapper mapper;

        public VillaController(IVillaService villaService,IMapper mapper)
        {
            this.villaService = villaService;
            this.mapper = mapper;
        }



        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDTO> list = new();
            var response = await villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if(response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        [Authorize]
        public async Task<IActionResult> CreateVilla()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await villaService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa Added Succssefully";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            TempData["error"] = "Error While Deleting";
            return View(model);
        }

        [Authorize(Roles ="admin")]
        public async Task<IActionResult> UpdateVilla(int id)
        {
            var response = await villaService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                VillaDTO villa = JsonConvert.DeserializeObject<VillaDTO>(response.Result.ToString());

                return View(mapper.Map<VillaUpdateDTO>(villa));
            }
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await villaService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa Updated Succssefully";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            TempData["error"] = "Error While Deleting";
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            var response = await villaService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                VillaDTO villa = JsonConvert.DeserializeObject<VillaDTO>(response.Result.ToString());

                return View(villa);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVilla(VillaDTO villa)
        {
                var response = await villaService.DeleteAsync<APIResponse>(villa.Id, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa Deleted Succssefully";
                    return RedirectToAction(nameof(IndexVilla));
                }
            TempData["error"] = "Error While Deleting";
            return View(villa);
        }
    }
}
