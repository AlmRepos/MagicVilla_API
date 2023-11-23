using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Models.ViewModels;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using MagicVilla_Utility;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService villaNumber;
        private readonly IMapper mapper;
        private readonly IVillaService villaService;

        public VillaNumberController(IVillaNumberService villaNumber,IMapper mapper,IVillaService villaService)
        {
            this.villaNumber = villaNumber;
            this.mapper = mapper;
            this.villaService = villaService;
        }


        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> list = new();
            var response = await villaNumber.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if(response!= null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(response.Result.ToString());
            }
            return View(list);
        }


        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateVillaNumber()
        {
            VillaNumberCreateVM VM = new();
            var response = await villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                VM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(response.Result.ToString()).Select(v=>new SelectListItem()
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                });
            }
            return View(VM);
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM VM)
        {
            if(ModelState.IsValid)
            {
                var response = await villaNumber.CreateAsync<APIResponse>(VM.createDTO, HttpContext.Session.GetString(SD.SessionToken));
                if(response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    if(response.ErrorMessages.Count>0)
                    {
                        ModelState.AddModelError("ErrorMessager", response.ErrorMessages.FirstOrDefault());
                    }
                }

            }
            var resp = await villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (resp != null && resp.IsSuccess)
            {
                VM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(resp.Result.ToString()).Select(v => new SelectListItem()
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                });
            }
            return View(VM);
        }



        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVillaNumber(int villaNo)
        {
            VillaNumberUpdateVM VM = new();
            var response = await villaNumber.GetAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD.SessionToken));
            if(response!= null && response.IsSuccess)
            {
                VillaNumberDTO dto = JsonConvert.DeserializeObject<VillaNumberDTO>(response.Result.ToString());
                VM.updateDTO = mapper.Map<VillaNumberUpdateDTO>(dto);
            }

            response = await villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                VM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(response.Result.ToString()).Select(v => new SelectListItem()
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                });

                return View(VM);
            }
            return NotFound();
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM VM)
        {
            if (ModelState.IsValid)
            {
                var response = await villaNumber.UpdateAsync<APIResponse>(VM.updateDTO, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessager", response.ErrorMessages.FirstOrDefault());
                    }
                }

            }
            var resp = await villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (resp != null && resp.IsSuccess)
            {
                VM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(resp.Result.ToString()).Select(v => new SelectListItem()
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                });
            }
            return View(VM);
        }



        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVillaNumber(int villaNo)
        {
            VillaNumberDeleteVM VM = new();
            var response = await villaNumber.GetAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                VillaNumberDTO dto = JsonConvert.DeserializeObject<VillaNumberDTO>(response.Result.ToString());
                VM.Villa = dto;
            }

            response = await villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                VM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(response.Result.ToString()).Select(v => new SelectListItem()
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                });

                return View(VM);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM model)
        {
            var response = await villaNumber.DeleteAsync<APIResponse>(model.Villa.VillaNo, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }
            return View(model);
        }

    }
}
