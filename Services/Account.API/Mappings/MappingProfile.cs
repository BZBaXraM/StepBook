using BuildingBlocks.Extensions;

namespace Account.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, MemberDto>()
            .ForMember(d => d.Age, o => o.MapFrom(s => s.DateOfBirth.CalculateAge()))
            .ForMember(d => d.PhotoUrl, o =>
                o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain)!.Url));
        CreateMap<Photo, PhotoDto>();
        CreateMap<MemberUpdateDto, User>();
        CreateMap<RegisterRequestDto, User>();
    }
}