using Architecture.Contracts;
using Architecture.Domain.Entities;
using AutoMapper;

namespace Architecture
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<Member, MemberCreateDTO>().ReverseMap();
            //CreateMap<Member, MemberCreateDTO>().ForMember(x => x.Name, opt => opt.MapFrom(src => $"{src.Name} some string..."));
        }
    }
}
