using Architecture.Contracts;
using Architecture.Core.Services.Members;
using Architecture.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Architecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [Produces("application/json")]
        [HttpGet]
        [ProducesResponseType(typeof(MemberCreateDTO[]), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var members = await _membersService.Get();
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
            return Ok(result);
        }

        /// <summary>
        /// Check authorization
        /// </summary>
        /// <returns></returns>
        //[Authorize]
        [HttpGet("auth")]
        public async Task<IActionResult> TestAuth()
        {
            var user = User;

            return Ok("Your authorize");
        }
    }
}
