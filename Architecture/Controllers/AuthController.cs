using Architecture.Models;
using Architecture.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.Swagger.Annotations;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Architecture.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly IUserService _userService;

        public AuthController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }
        
        [AllowAnonymous]
        [HttpPost(nameof(Auth))]
        [SwaggerRequestExample(typeof(LoginModel), typeof(TokenExampleRequest))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TokenExampleResponce))] // don't work
        public IActionResult Auth([FromBody] LoginModel data)
        {
            bool isValid = _userService.IsValidUserInformation(data);
            if (isValid)
            {
                var tokenString = GenerateJwtToken(data.UserName);
                return Ok(new TokenModel { Token = tokenString, Message = "Success" });
            }
            return BadRequest("Please pass the valid Username and Password");
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(nameof(GetResult))]
        public IActionResult GetResult()
        {
            //var a = User;
            //var user = User.Claims.Where(x => x.Type == "id").Select(x => x.Value).FirstOrDefault();
            return Ok("API Validated");
        }

        /// <summary>
        /// Generate JWT Token after successful login.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        private string GenerateJwtToken(string userName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                { 
                    new Claim("userName", userName),
                    new Claim("id", "7"),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
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
        public string Token { get; set; }
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
                UserName = "Jay",
                Password = "123456"
            };
        }
    }
}
