using AutoMapper;
using Azure;
using MagicVilla.Data;
using MagicVilla.Models;
using MagicVilla.Models.DTO;
using MagicVilla.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        protected APIResponse response;
        private readonly IVillaRepository dbVilla;
        private readonly IMapper mapper;

        public VillaAPIController(IVillaRepository dbVilla, IMapper mapper)
        {
            this.dbVilla = dbVilla;
            this.mapper = mapper;
            response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                IEnumerable<Villa> villas = await dbVilla.GetAllAsync();
                response.Result = mapper.Map<List<VillaDTO>>(villas);
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { ex.ToString() };

            }
            return response;

        }



        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }

                var villa = await dbVilla.GetAsync(v => v.Id == id);

                if (villa == null)
                {
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return NotFound(response);
                }

                response.Result = mapper.Map<VillaDTO>(villa);
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return response;
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }
                if (await dbVilla.GetAsync(v => v.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("CustomError", "Name is Already Exist");
                    return BadRequest(ModelState);
                }

                Villa villa = mapper.Map<Villa>(createDTO);

                await dbVilla.CreateAsync(villa);
                response.Result = mapper.Map<VillaDTO>(villa);
                response.StatusCode = System.Net.HttpStatusCode.Created;

                return CreatedAtRoute("GetVilla", new { id = villa.Id }, response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return response;
        }



        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var villa = await dbVilla.GetAsync(v => v.Id == id);

                if (villa == null)
                {
                    return NotFound();
                }

                await dbVilla.RemoveAsync(villa);

                response.StatusCode = System.Net.HttpStatusCode.NoContent;
                response.IsSuccess = true;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return response;
        }



        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO UpdateDTO)
        {
            try
            {
                if (UpdateDTO == null || id != UpdateDTO.Id)
                {
                    return BadRequest();
                }

                Villa villa = mapper.Map<Villa>(UpdateDTO);

                await dbVilla.UpdateAsync(villa);

                response.StatusCode = System.Net.HttpStatusCode.NoContent;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return response;
        }




        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (id == 0 || patchDTO == null)
            {
                return BadRequest();
            }

            var villa = await dbVilla.GetAsync(v => v.Id == id, false);

            if (villa == null)
            {
                return NotFound();
            }

            VillaUpdateDTO UpdateDTO = mapper.Map<VillaUpdateDTO>(villa);

            patchDTO.ApplyTo(UpdateDTO, ModelState);

            Villa Model = mapper.Map<Villa>(UpdateDTO);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await dbVilla.UpdateAsync(Model);

            return NoContent();
        }
    }
}
