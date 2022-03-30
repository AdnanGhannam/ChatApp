using AutoMapper;
using ChatVia.Domain.Entities;
using ChatVia.Shared.ResponseDtos;

namespace ChatVia.Server.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, AppUserByIdDto>();
            CreateMap<AppUser, AppUserListDto>();
            CreateMap<Chat, ChatListDto>()
                .ForMember(e => e.Message, config => config.MapFrom(src => src.Messages.First()))
                .ForMember(e => e.Member, config => config.Ignore())
                .ForMember(e => e.IsActive, config => config.Ignore());
            CreateMap<Chat, ChatDto>()
                .ForMember(e => e.Member, config => config.Ignore())
                .ForMember(e => e.IsMuted, config => config.Ignore());
            CreateMap<Message, MessageDto>();
        }
    }
}
