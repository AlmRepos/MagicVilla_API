using MagicVilla.Models;
using MagicVilla.Models.DTO;
using MagicVilla.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository user;
        APIResponse response;
        public UsersController(IUserRepository user) 
        {
            this.user = user;
            response = new();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO requestDTO)
        {
            var loginResponse = await user.Login(requestDTO);
            if(loginResponse.User == null ||string.IsNullOrEmpty(loginResponse.Token))
            {
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessages.Add("Username or Password is incorrect");

                return BadRequest(response);
            }
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Result = loginResponse;

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO requestDTO)
        {
            bool Uniqe = user.IsUniqeUser(requestDTO.UserName);
            if (!Uniqe)
            {
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessages.Add("Username already exist");

                return BadRequest(response);
            }
            var registeredUser = await user.Register(requestDTO);
            if(registeredUser == null)
            {
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessages.Add("Error while registering");

                return BadRequest(response);
            }

            response.StatusCode = System.Net.HttpStatusCode.OK;

            return Ok(response);
        }
    }
}
