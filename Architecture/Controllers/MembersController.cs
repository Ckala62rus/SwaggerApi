using Architecture.Contracts;
using Architecture.Core.Services.Members;
using Architecture.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;
using System.Threading.Tasks;

namespace Architecture.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MembersController : Controller
    {
        private readonly IMembersService _membersService;
        private readonly IMapper _mapper;

        public MembersController(
            IMembersService membersService,
            IMapper mapper)
        {
            _membersService = membersService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all members
        /// </summary>
        /// <remarks>
        /// Get /members
        /// </remarks>
        /// <returns>Returns Members list</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpGet]
        [ProducesResponseType(typeof(MemberCreateDTO[]), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get()
        {
            var members = await _membersService.Get();
            Log.Information("=============================================================");
            Log.Information("{@members}", members);
            Log.Information("=============================================================");
            var results = _mapper.Map<Member[], MemberCreateDTO[]>(members);
            return Ok(results);
        }

        /// <summary>
        /// Create new member
        /// </summary>
        /// <param name="newMember"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(typeof(MemberCreateDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create(MemberCreateDTO newMember)
        {
            var member = _mapper.Map<Member>(newMember);

            var memberId = await _membersService.Create(member);

            //return Ok(memberId);
            return Ok(newMember);
        }

        /// <summary>
        /// Get member by YouTubeUserId
        /// </summary>
        /// <param name="youtubeUserId"></param>
        /// <returns></returns>
        [HttpGet("{youtubeUserId}")]
        [ProducesResponseType(typeof(MemberCreateDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromRoute] string youtubeUserId)
        {
            var member = await _membersService.Get(youtubeUserId);
            var result = _mapper.Map<Member, MemberCreateDTO>(member);

            if (result == null) return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Check authorization
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("auth")]
        public async Task<IActionResult> TestAuth()
        {
            var user = User;

            return Ok("Your authorize");
        }
    }
}
