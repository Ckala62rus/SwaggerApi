using Architecture.Core.Services.Users;
using Architecture.Domain.Entities;
using Architecture.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Architecture.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly IUsersService _userService;

        public AuthController(IConfiguration configuration, IUsersService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }
        
        [AllowAnonymous]
        [HttpPost(nameof(Auth))]
        [SwaggerRequestExample(typeof(TokenModel), typeof(TokenExampleRequest))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TokenExampleResponce))] // don't work
        public async Task<IActionResult> Auth([FromBody] UserRegistrationModel data)
        {
            var user = await _userService.GetUserByEmail(data.Email);

            if (user == null)
            {
                return NotFound("User not Found");
            }

            var inputPasswordHash = _userService.hashPassword(data.Password);

            if (inputPasswordHash == user.Password)
            {
                var tokenString = GenerateJwtToken(user.Email, user.Id);

                user.RefreshToken = _userService.GenerateRefreshToken();
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);
                var tokenRefresh = await _userService.Update(user);

                return Ok(new TokenModel { Token = tokenString, Message = "Success", RefreshToken = tokenRefresh.RefreshToken });
            }

            return BadRequest("Incorrect password!");
        }

        [AllowAnonymous]
        [HttpPost(nameof(Registration))]
        [SwaggerRequestExample(typeof(TokenModel), typeof(TokenExampleRequest))]
        public async Task<IActionResult> Registration([FromBody] UserRegistrationModel userRegistrationModel)
        {
            var userCreated = await _userService.Create(new User
            {
                Email = userRegistrationModel.Email,
                Password = userRegistrationModel.Password,
            });

            var user = await _userService.GetUser(userCreated);
            var tokenString = GenerateJwtToken(user.Email, user.Id);

            return Ok(new TokenModel
            {
                User = user,
                Token = tokenString,
                Message = "User was created"
            });
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost(nameof(RefreshToken))]
        [SwaggerRequestExample(typeof(TokenModel), typeof(TokenRefreshExampleRequest))]
        public async Task<IActionResult> RefreshToken([FromBody] TokenApiModel tokenApiModel)
        {
            if (tokenApiModel is null)
                return BadRequest("Invalid client request");

            var userId = User.Claims.Where(x => x.Type == "id").Select(x => x.Value).FirstOrDefault();

            var user = await _userService.GetUser(Int32.Parse(userId));

            string refreshToken = tokenApiModel.RefreshToken;

            if (user is null || String.IsNullOrEmpty(refreshToken) || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return Unauthorized("User not found or refresh token empty");
            }

            user.RefreshToken = _userService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);

            var token = GenerateJwtToken(user.Email, user.Id);
            var refreshedToken = await _userService.Update(user);

            return Ok(new TokenModel { Token = token, Message = "Token was refreshed", RefreshToken = refreshedToken.RefreshToken });
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(nameof(GetResult))]
        public IActionResult GetResult()
        {
            //var a = User;
            var user = User.Claims.Where(x => x.Type == "id").Select(x => x.Value).FirstOrDefault();
            return Ok("API Validated");
        }

        /// <summary>
        /// Generate JWT Token after successful login.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        private string GenerateJwtToken(string userName, int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                { 
                    new Claim("userName", userName),
                    new Claim("id", userId.ToString()),
                }),
                Expires = DateTime.Now.AddMinutes(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    /// <summary>
    /// Для вывода примера ответа в документации Swagger
    /// </summary>
    public class TokenExampleResponce : IExamplesProvider<TokenModel>
    {
        public TokenModel GetExamples()
        {
            return new TokenModel
            {
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9" +
                ".eyJ1c2VyTmFtZSI6IkpheSIsImlkIjoiNyIsIm5iZiI6MTY2MzMxMzQwNSwiZXhwIjoxNjYzMzE3MDA1LCJpYXQiOjE2NjMzMTM0MDUsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0MzQxIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo0MjAwIn0" +
                ".n8ekLaHVV-mvXfpUMH8V7hd3ToGnPLgEMX4e6f_43fk",
                Message = "Success"
            };
        }
    }

    /// <summary>
    /// Модель для примера ответа, при получении токена
    /// </summary>
    public class TokenModel
    {
        public User User { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// Для вывода примера входных параметров контроллера в документации Swagger
    /// </summary>
    public class TokenExampleRequest : IExamplesProvider<LoginModel>
    {
        public LoginModel GetExamples()
        {
            return new LoginModel
            {
                Email = "admin@mail.ru",
                Password = "123456"
            };
        }
    }

    public class TokenRefreshExampleRequest : IExamplesProvider<TokenModel>
    {
        public TokenModel GetExamples()
        {
            return new TokenModel
            {
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9" +
                ".eyJ1c2VyTmFtZSI6IkpheSIsImlkIjoiNyIsIm5iZiI6MTY2MzMxMzQwNSwiZXhwIjoxNjYzMzE3MDA1LCJpYXQiOjE2NjMzMTM0MDUsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0MzQxIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo0MjAwIn0" +
                ".n8ekLaHVV-mvXfpUMH8V7hd3ToGnPLgEMX4e6f_43fk",
                RefreshToken = "some string token"
            };
        }
    }

    public class TokenApiModel
    {
        public string RefreshToken { get; set; }
    }
}
