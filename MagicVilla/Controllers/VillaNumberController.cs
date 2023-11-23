using AutoMapper;
using MagicVilla.Models;
using MagicVilla.Models.DTO;
using MagicVilla.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MagicVilla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaNumberController : ControllerBase
    {
        private readonly IVillaNumberRepository repo;
        private readonly IMapper mapper;
        private readonly IVillaRepository villaRepository;
        private APIResponse response;

        public VillaNumberController(IVillaNumberRepository repo, IMapper mapper,IVillaRepository villaRepository)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.villaRepository = villaRepository;
            response = new();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAll()
        {
            try
            {
                IEnumerable<VillaNumber> villaNumbers = await repo.GetAllAsync(includeProperties:"Villa");

                response.Result = mapper.Map<List<VillaNumberDTO>>(villaNumbers);
                response.StatusCode = System.Net.HttpStatusCode.OK;

                return Ok(response);
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { ex.ToString() };
                return response;
            }
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id:int}",Name ="GetVillaNumber")]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {
                if(id == 0)
                {
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }
                var villanumber = await repo.GetAsync(v => v.VillaNo == id, includeProperties: "Villa");

                if(villanumber == null)
                {
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return NotFound(response); 
                }
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Result = mapper.Map<VillaNumberDTO>(villanumber);
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.ErrorMessages = new List<string>() { ex.ToString() };
                response.IsSuccess = false;
            }

            return response;
        }



        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost(Name =("CreateVillaNumber"))]
        public async Task<ActionResult<APIResponse>> CreateVillaNummber([FromBody] VillaNumberCreateDTO createDTO)
        {
            try
            {
                if(await repo.GetAsync(v=>v.VillaNo == createDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "VillaNumber Already Exist! ");
                    return BadRequest(ModelState);
                }

                if(await villaRepository.GetAsync(v=>v.Id == createDTO.VillaId) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa Id is Invalid! ");
                    return BadRequest(ModelState);
                }

                if(createDTO == null)
                {
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }
                var villa = mapper.Map<VillaNumber>(createDTO);
                await repo.CreateAsync(villa);

                response.StatusCode = System.Net.HttpStatusCode.Created;
                response.Result = mapper.Map<VillaNumberDTO>(villa);

                return CreatedAtRoute("GetVillaNumber", new { id = villa.VillaNo }, response); 
            }
            catch (Exception ex)
            {
                response.ErrorMessages = new List<string>() { ex.ToString() };
                response.IsSuccess = false;
            }
            return response;
        }



        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}", Name = ("DeleteVillaNumber"))]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            try
            { 
                if(id == 0 )
                {
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }

                var villa = await repo.GetAsync(v => v.VillaNo == id);

                if(villa == null)
                {
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return NotFound(response);
                }

                await repo.RemoveAsync(villa);
                response.StatusCode = System.Net.HttpStatusCode.NoContent;

                return Ok(response);
            }
            catch(Exception ex)
            {
                response.ErrorMessages = new List<string>() { ex.ToString() };
                response.IsSuccess = false;
            }
            return response;
        }



        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}",Name =("UpdateVillaNumber"))]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO updateDTO)
        {
            try
            {
                if(updateDTO == null || id != updateDTO.VillaNo)
                {
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }
                if (await villaRepository.GetAsync(v => v.Id == updateDTO.VillaId) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa Id is Invalid! ");
                    return BadRequest(ModelState);
                }

                var VillaNumber = mapper.Map<VillaNumber>(updateDTO);

                await repo.UpdateVillaNumberAsync(VillaNumber);

                response.Result = mapper.Map<VillaNumberDTO>(VillaNumber);
                response.StatusCode = System.Net.HttpStatusCode.NoContent;

                return Ok(response);
            }
            catch(Exception ex)
            {
                response.ErrorMessages = new List<string>() { ex.ToString() };
                response.IsSuccess = false;
            }
            return response;
        }

    }
}
