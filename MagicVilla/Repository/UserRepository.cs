using MagicVilla.Data;
using MagicVilla.Models;
using MagicVilla.Models.DTO;
using MagicVilla.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly Context db;
        string secretKey;
        public UserRepository(Context db, IConfiguration configuration)
        {
            this.db = db;
            secretKey = configuration.GetValue<string>("APISettings:Secret");
        }

        public bool IsUniqeUser(string UserName)
        {
            var user = db.LocalUsers.FirstOrDefault(u => u.UserName == UserName);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO requestDTO)
        {
            var user = db.LocalUsers.FirstOrDefault(u => u.UserName.ToLower() == requestDTO.UserName.ToLower() &&
            u.Password == requestDTO.Password);
            //if(user.Password == requestDTO.Password)
            //{

            //}

            if (user == null)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDTO responseDTO = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                User = user
            };

            return responseDTO;
        }

        public async Task<LocalUser> Register(RegisterationRequestDTO requestDTO)
        {
            LocalUser user = new LocalUser()
            {
                Name = requestDTO.Name,
                Password = requestDTO.Password,
                Role = requestDTO.Role,
                UserName = requestDTO.UserName
            };

            db.LocalUsers.Add(user);
            await db.SaveChangesAsync();
            user.Password = "##########";
            return user;
        }
    }
}
