using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShopping.API.Data;
using OnlineShopping.API.Dtos;
using OnlineShopping.API.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace OnlineShopping.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();            
            userForRegisterDto.FirstName=userForRegisterDto.FirstName.ToString();
            userForRegisterDto.LastName=userForRegisterDto.LastName.ToString();
            userForRegisterDto.Email=userForRegisterDto.Email.ToString();
            userForRegisterDto.PhoneNo=userForRegisterDto.PhoneNo.ToString();
            userForRegisterDto.Gender=userForRegisterDto.Gender.ToString();
            userForRegisterDto.Country=userForRegisterDto.Country.ToString();

            if (await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("User Name already exist");

            var userToCreate = new User()
            {
                UserName = userForRegisterDto.Username,
                FirstName=userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
                Email=userForRegisterDto.Email,
                PhoneNo = userForRegisterDto.PhoneNo,
                Gender=userForRegisterDto.Gender,
                Country=userForRegisterDto.Country
            };

            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
               new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
               new Claim(ClaimTypes.Name,userFromRepo.UserName)
           };

            var key = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor=new SecurityTokenDescriptor
            {
                Subject=new ClaimsIdentity(claims),
                Expires=DateTime.Now.AddDays(1),
                SigningCredentials=creds

            };

            var tokenHandler=new JwtSecurityTokenHandler();

            var token=tokenHandler.CreateToken(tokenDescriptor);

            return Ok (new {
                token=tokenHandler.WriteToken(token)
            });
        }
    }
}